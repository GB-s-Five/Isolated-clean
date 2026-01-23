using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    //[SerializeField] private LayerMask interactionLayer;

    [Header("UI Elements")]
    [SerializeField] private CanvasGroup interactionUI;
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private Image interactionIcon;

    [Header("Inspection Panel")]
    [SerializeField] private GameObject inspectionPanel; // panel global de inspección

    [SerializeField] private PlayerMovement playerMovement; // Movimiento jugador
    [SerializeField] private PlayerLook playerLook; // Movimiento jugador
    [SerializeField] private Volume postProcessingVolume;


    private PlayerControls controls;
    private Camera cam;
    private IInteractable currentTarget;

    // Awake(): Se ejecuta al inicializar el script y configura el action map
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => TryInteract();
    }

    // OnEnable(): Se ejecuta al habilitar el script y activa el action map
    private void OnEnable() => controls.Player.Enable();

    // OnDisable(): Se ejecuta al deshabilitar el script y desactiva el action map
    private void OnDisable() => controls.Player.Disable();

    // Start(): Se ejecuta al iniciar el juego
    // Configura la cámara y lanza InvokeRepeating para el raycast
    private void Start()
    {
        cam = Camera.main;

        // Llamamos al raycast repetidamente cada 0.1s
        InvokeRepeating(nameof(CheckForInteractable), 0f, 0.1f);

        HideUI();
    }

    // CheckForInteractable():
    // Se ejecuta periódicamente usando InvokeRepeating para detectar objetos interactuables
    private void CheckForInteractable()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            //Debug.Log(hit.collider.name);
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.IsInteractable())
                {
                    currentTarget = interactable;

                    ShowUI(interactable.GetInteractionText());
                    return;
                }
            }
        }

        currentTarget = null;
        HideUI();
    }

    // TryInteract(): Se ejecuta al pulsar el botón de interacción
    // Llama al método Interact() del objeto actual si existe
    private void TryInteract()
    {
        if (currentTarget != null)
        {
                // Si es inspeccionable, pasarle el panel y el PlayerMovement
            if (currentTarget is InspectableObject inspectable)
            {
                if (inspectionPanel != null)
                    inspectable.SetInspectionPanel(inspectionPanel);

                if (playerMovement != null)
                    inspectable.SetPlayerMovement(playerMovement);
                    
                if (postProcessingVolume != null)
                    inspectable.SetPostProcessingVolume(postProcessingVolume);

                if (playerLook != null)
                    inspectable.SetPlayerLook(playerLook);
            }

            
            currentTarget.Interact();
        }
    }

    // ShowUI(): Muestra el texto y el icono en el HUD
    private void ShowUI(string text)
    {
        interactionText.text = "Press<sprite name=\"LeftClick\">to " + text;
        interactionUI.alpha = 1;
        interactionUI.blocksRaycasts = true;
    }

    // HideUI(): Oculta el HUD cuando no hay objeto seleccionado
    private void HideUI()
    {
        interactionUI.alpha = 0;
        interactionUI.blocksRaycasts = false;
    }
}
