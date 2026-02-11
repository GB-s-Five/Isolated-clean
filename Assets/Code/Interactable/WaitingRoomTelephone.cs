using UnityEngine;
using System.Collections;


public class WaitingRoomTelephone : MonoBehaviour , IInteractable
{
    [Header("Clip Telefono (arrastrar AudioClip)")]
    [SerializeField] private AudioClip telephoneAudio1;  // ← Aquí arrastras un CLIP de audio
    
    [Header("Required Inspection ID")]
    [SerializeField] private string requiredInspectionID = "Rayos";
     
    [Header("Inspectable Object ID")]
    [SerializeField] private string objectID = "TelephoneHeared";

    [SerializeField] private AudioClip ringring;
    private bool hasInteracted;

    private AudioSource audioSource;
    private TriggerAudioThenAlarma triggerAudioThenAlarma;
    [SerializeField] private FootstepsController footstepsController; // 

    [SerializeField] private HeadBobSystem headBobSystem; // 

    [SerializeField] private PlayerMovement playerMovement;


    public string GetInteractionText() => "use Telephone";
    private void Awake()
    {
        hasInteracted = false;
        triggerAudioThenAlarma = GetComponent<TriggerAudioThenAlarma>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (!string.IsNullOrEmpty(requiredInspectionID) &&
            !PlayerProgress.Instance.HasInspected(requiredInspectionID))
        {
            Debug.Log($"No puedes usar el teléfono, necesitas inspeccionar {requiredInspectionID} primero.");
            return;
        }

        Debug.Log("El jugador tiene la ID requerida, se reproduce el audio.");
        StartCoroutine(PlayTelephoneAudio1());
        
    }
    
    
    private IEnumerator PlayTelephoneAudio1()
    {
        
        if (footstepsController != null) {
            footstepsController.enabled = false; //bloquea el movimiento del jugador
            Debug.Log($"{name}: Bloquea el movimiento");
        }

        if (headBobSystem != null) {
            headBobSystem.enabled = false; //bloquea el movimiento del jugador
            Debug.Log($"{name}: Bloquea el movimiento");
        }

        if (playerMovement != null) {
            playerMovement.enabled = false; //bloquea el movimiento del jugador
            Debug.Log($"{name}: Bloquea el movimiento");
        }

        if (audioSource == null)
           yield break;
        audioSource.loop = false;
        audioSource.Stop();
        PlayerProgress.Instance.RegisterInspection(objectID);

        //Si no es nulo, se ejecuta una sola vez el clip de audio
        if (telephoneAudio1 != null)
        {
            audioSource.PlayOneShot(telephoneAudio1);
            hasInteracted = true;
        }
        Debug.Log("Telephone audio finished. Alarm system enabled!");
        
        //Espera hasta que termine el sonido / clip de audio
        yield return new WaitForSeconds(telephoneAudio1.length);

        triggerAudioThenAlarma.ActivateAlarm();

  

    }
    public void PlayRing()
    {
        //Creamos un AudioSource automáticamente si no existe
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.clip = ringring;
        audioSource.Play();

    }

    public bool IsInteractable()
    {
        return (PlayerProgress.Instance.HasInspected(requiredInspectionID) && !hasInteracted);
        
    }
}
