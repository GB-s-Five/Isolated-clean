using UnityEngine;
using System.Collections;

public class ComputerInteractable : MonoBehaviour, IInteractable
{
    [Header("Audio")]
    [SerializeField] private AudioSource doctorAudio;
    public SOSubtitle soSubtitle;

    [Header("Machine to unlock after audio")]
    [SerializeField] private XRMachineInteractable machine;

    [Header("Required Inspection ID")]
    [SerializeField] private string requiredInspectionID = "NoteOnTable";

    [Header("Door to lock during audio")]
    [SerializeField] private DoorController doorToLock;

    
    private bool hasInteracted = false;

    private void Awake()
    {
        doctorAudio = GetComponent<AudioSource>();
        if(soSubtitle == null) Debug.LogWarning("soSubtitle es nulo en TriggerSonido:"+ gameObject.ToString());
        else
        {
            if (soSubtitle.audioClip != null)
                doctorAudio.clip = soSubtitle.audioClip;
            else
                Debug.LogWarning("Audioclip es nulo en TriggerSonido:"+ gameObject.ToString());
        }
        doctorAudio.playOnAwake = false;
    }
    public string GetInteractionText() => "use computer";

    public void Interact()
    {
        if (hasInteracted)
            return;

        if (!string.IsNullOrEmpty(requiredInspectionID) &&
            !PlayerProgress.Instance.HasInspected(requiredInspectionID))
            return;

        hasInteracted = true;

        //Cerrar y bloquear puerta
        // Cerrar y bloquear puerta
        if (doorToLock != null)
        {
            if (doorToLock.isOpen)
            {
                doorToLock.Toggle();
            }
            doorToLock.LockDoor();
        }

        StartCoroutine(PlayDoctorAudio());
    }

    public bool IsInteractable()
    {
        return !hasInteracted;
    }


    private IEnumerator PlayDoctorAudio()
    {
        // Bloquear la máquina por seguridad antes de iniciar el audio
        if (machine != null)
        {
            machine.SetInteractable(false);
            //Debug.Log("Machine locked before doctor audio starts.");
        }

        doctorAudio.Play();
        SubtitleManager.Instance.Show(soSubtitle);
        yield return new WaitForSeconds(doctorAudio.clip.length);

        if (machine != null)
        {
            machine.SetInteractable(true);
            //Debug.Log("Doctor audio finished. Machine unlocked!");
        }
    }
}