using UnityEngine;
using System.Collections;

public class XRMachineInteractable : MonoBehaviour, IInteractable
{
    [Header("Audio")]
    [SerializeField] private AudioSource machineAudio;

    [Header("Trigger to activate after machine")]
    [SerializeField] private GameObject childTrigger;

    private bool isInteractable = false;
    private bool hasInteracted = false; // evita repetir la interacción
    [SerializeField] private PlayerMovement playerMovement; //para el movimiento del jugador

    

    private void Start()
    {
        isInteractable = false; // bloqueada al inicio
        hasInteracted = false;
        //Debug.Log($"{name}: Machine interaction set to FALSE at Start.");
    }

    public string GetInteractionText()
    {
        if (hasInteracted)
        {
            isInteractable = false;
            return ""; // no mostrar mensaje después de usar
        }
        return  "use machine";
    }

    public void Interact()
    {
        if (!isInteractable)
        {
            //Debug.Log($"{name}: Attempted to interact but machine is locked.");
            return;
        }

        if (hasInteracted)
        {
            //Debug.Log($"{name}: Machine already interacted.");
            return;
        }

        hasInteracted = true;
        isInteractable = false; // bloquear después de usar
        if (playerMovement != null) {
            playerMovement.enabled = false; //bloquea el movimiento del jugador
            Debug.Log($"{name}: Bloquea el movimiento");
        }
        
        //Debug.Log($"{name}: Machine interacted!");

        if (machineAudio != null)
        {
            machineAudio.Play();
            StartCoroutine(PlayXMachineAudio());
            Debug.Log($"{name}: Playing machine audio.");
        }

        
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }

    // PlayXMachineAudio():
    // Espera la duracion del audio antes de habilitar el trigger
    private IEnumerator PlayXMachineAudio()
    {

        machineAudio.Play();
        yield return new WaitForSeconds(machineAudio.clip.length);
        if (playerMovement != null) {
            playerMovement.enabled = true; //habilito el movimiento del jugador
            Debug.Log($"{name}: Habilita el movimiento");
        }
        Debug.Log($"{name}: habilita movement");
        
        if (childTrigger != null)
        {
            childTrigger.SetActive(true);
            Debug.Log($"{name}: Child trigger activated.");
        }
    }
    
    public void SetInteractable(bool value)
    {
        if (hasInteracted)
        {
            isInteractable = false; // no se puede desbloquear después de usar
        }
        else
        {
            isInteractable = value;
        }

        //Debug.Log($"{name}: SetInteractable called. Now isInteractable = {isInteractable}");
    }
}