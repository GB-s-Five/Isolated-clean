using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InspectableObject : MonoBehaviour, IInteractable
{
    [Header("Inspectable Object ID")]
    [SerializeField] private string objectID = "InspectableItem";

    [Header("Inspection Settings")]
    public float distanceFromCamera = 0.8f;
    public float moveDuration = 0.35f;
    public float rotationSpeed = 120f;
    public float floatStrength = 0.025f;
    public float floatSpeed = 2f;
    public float inspectionScale = 0.3f; // Escala reducida durante inspección

    private GameObject inspectionPanel;
    private CanvasGroup panelCG;
    private GameObject interactionUIObject;

    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private Transform playerCamera;

    private BoxCollider boxCollider; 
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Vector3 originalScale;
    private bool inspecting = false;

    // URP Volume
    private Volume postVolume;
    private Bloom bloom;
    private DepthOfField dof;
    private ChromaticAberration chroma;
    private Vignette vignette;
    private bool isInspectionable=true;

    // -------------------------------
    // IInteractable: texto
    // -------------------------------
    public string GetInteractionText() => "inspect";

    // -------------------------------
    // MÉTODOS PARA RECIBIR REFERENCIAS
    // -------------------------------
    public void SetInspectionPanel(GameObject panel)
    {
        inspectionPanel = panel;
        panelCG = panel?.GetComponent<CanvasGroup>();

        if (inspectionPanel != null)
        {
            inspectionPanel.SetActive(false);
            if (panelCG != null)
                panelCG.alpha = 0f;
        }
    }

    public void SetPlayerMovement(PlayerMovement movement) => playerMovement = movement;
    public void SetPlayerLook(PlayerLook look) => playerLook = look;
    public void SetInteractionUI(GameObject ui) => interactionUIObject = ui;

    public void SetPostProcessingVolume(Volume vol)
    {
        postVolume = vol;
        if (postVolume != null && postVolume.profile != null)
        {
            postVolume.profile.TryGet(out bloom);
            postVolume.profile.TryGet(out dof);
            postVolume.profile.TryGet(out chroma);
            postVolume.profile.TryGet(out vignette);
            postVolume.enabled = false; // por defecto desactivado
            //postVolume.weight = 0f;
        }
    }

    public void HideUI(GameObject ui)
    {
        if (ui != null)
            ui.SetActive(false);
    }

    private void Start()
    {
        playerCamera = Camera.main.transform;
        boxCollider = GetComponent<BoxCollider>(); 

        if (inspectionPanel != null)
        {
            inspectionPanel.SetActive(false);
            if (panelCG != null) panelCG.alpha = 0f;
        }
    }

    private void Update()
    {
        if (!inspecting) return;

        // Rotación con botón central del ratón
        if (Mouse.current.middleButton.isPressed)
        {
            float mx = Mouse.current.delta.x.ReadValue();
            float my = Mouse.current.delta.y.ReadValue();

            transform.Rotate(Vector3.up, -mx * rotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, my * rotationSpeed * Time.deltaTime, Space.World);
        }

        // Salir con click derecho
        if (Mouse.current.rightButton.wasPressedThisFrame)
            StartCoroutine(EndInspection());

        // Flotación
        transform.position += Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatStrength * Time.deltaTime;
    }

    // -------------------------------
    // INICIAR INSPECCIÓN
    // -------------------------------
    public void Interact()
    {
        if (!inspecting)
        {
            PlayerProgress.Instance.RegisterInspection(objectID);
            StartCoroutine(StartInspection());
        }
    }

    public bool IsInteractable()
    {
        return isInspectionable;
    }

    private IEnumerator StartInspection()
    {
        inspecting = true;

        originalPos = transform.position;
        originalRot = transform.rotation;
        originalScale = transform.localScale;

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerLook != null) playerLook.enabled = false;

        if (boxCollider != null) boxCollider.enabled = false;
        if (interactionUIObject != null) HideUI(interactionUIObject);

        if (inspectionPanel != null)
        {
            inspectionPanel.SetActive(true);
            if (panelCG != null) panelCG.alpha = 1f;
        }

        if (postVolume != null) postVolume.enabled = true;
        EnablePostFX(true);

        Vector3 targetPos = playerCamera.position + playerCamera.forward * distanceFromCamera;
        Quaternion targetRot = Quaternion.identity;
        Vector3 targetScale = originalScale * inspectionScale;

        float t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 startScale = transform.localScale;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float lerp = t / moveDuration;

            transform.position = Vector3.Lerp(startPos, targetPos, lerp);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, lerp);
            transform.localScale = Vector3.Lerp(startScale, targetScale, lerp);

            // Fade progresivo del volume y efectos
            if (postVolume != null) postVolume.weight = lerp;
            if (chroma != null) chroma.intensity.value = 0.4f * lerp;
            if (vignette != null)
            {
                vignette.intensity.value = 0.3f * lerp;
                vignette.smoothness.value = 0.3f * lerp;
            }

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;
        transform.localScale = targetScale;
        EnablePostFX(true);
    }

    // -------------------------------
    // FINALIZAR INSPECCIÓN
    // -------------------------------
    private IEnumerator EndInspection()
    {
        float t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 startScale = transform.localScale;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float lerp = t / moveDuration;

            transform.position = Vector3.Lerp(startPos, originalPos, lerp);
            transform.rotation = Quaternion.Slerp(startRot, originalRot, lerp);
            transform.localScale = Vector3.Lerp(startScale, originalScale, lerp);

            if (postVolume != null) postVolume.weight = 1f - lerp;
            if (chroma != null) chroma.intensity.value = 0.4f * (1f - lerp);
            if (vignette != null)
            {
                vignette.intensity.value = 0.3f * (1f - lerp);
                vignette.smoothness.value = 0.3f * (1f - lerp);
            }

            yield return null;
        }

        transform.position = originalPos;
        transform.rotation = originalRot;
        transform.localScale = originalScale;

        if (playerMovement != null) playerMovement.enabled = true;
        if (playerLook != null) playerLook.enabled = true;
        if (boxCollider != null) boxCollider.enabled = true;

        if (inspectionPanel != null)
        {
            if (panelCG != null) panelCG.alpha = 0f;
            inspectionPanel.SetActive(false);
        }

        if (postVolume != null)
        {
            //postVolume.weight = 0f;
            //postVolume.enabled = false;
        }

        //EnablePostFX(false);
        inspecting = false;
    }

    // -------------------------------
    // EFECTOS URP ON/OFF
    // -------------------------------
    private void EnablePostFX(bool enable)
    {
        if (bloom != null) bloom.active = enable;
        if (dof != null) dof.active = enable;
        if (chroma != null)
        {
            chroma.active = enable;
            if (enable) chroma.intensity.value = 0.4f;
        }
        if (vignette != null)
        {
            vignette.active = enable;
            if (enable)
            {
                vignette.intensity.value = 0.3f;
                vignette.smoothness.value = 0.3f;
            }
        }
    }
}
