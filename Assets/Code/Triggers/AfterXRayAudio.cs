using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Collider))]
public class AfterXRayAudio : MonoBehaviour
{
    [Header("Audio & ID")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string idToGive = "Rayos";

    [Header("Required Inspection ID")]
    [SerializeField] private string requiredInspectionID;

    [SerializeField] private WaitingRoomTelephone waitingRoomTelephone;
   
    [SerializeField] private DoorController doorToUnlock;

    private bool hasTriggered = false;
    public SOSubtitle soSubtitle;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(soSubtitle == null) Debug.LogWarning("soSubtitle es nulo en TriggerSonido:"+ gameObject.ToString());
        else
        {
            if (soSubtitle.audioClip != null)
                audioSource.clip = soSubtitle.audioClip;
            else
                Debug.LogWarning("Audioclip es nulo en TriggerSonido:"+ gameObject.ToString());
        }
        audioSource.playOnAwake = false;
        //childAudio.volume = 0f;
    }
    
    // OnTriggerEnter(Collider other):
    // Comprueba si tiene el id requerido, entonces reproduce el audio, y nos da otro id para salir.
    private void OnTriggerStay(Collider other) // Antes OnTriggerEnter // Se cambia a trigger stay ya que recibe el ID mientras esta dentro del trigger
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        // Comprobar ID requerido continuamente
        if (!string.IsNullOrEmpty(requiredInspectionID) &&
            !PlayerProgress.Instance.HasInspected(requiredInspectionID))
        {
            return;
        }

        TriggerSequence(); // Salta la secuencia del trigger 
    }

    private void TriggerSequence() // Funcion para hacer las cosas del trigger // Se mantiene igual que antes (las funciones estaban dentro del trigger enter)
    {
        if (audioSource != null)
        {
            audioSource.Play();
            SubtitleManager.Instance.Show(soSubtitle);
        }

        PlayerProgress.Instance.RegisterInspection(idToGive); // D

        Debug.Log("Checkpoint alcanzado");
        Checkpointmanager.Instance.SaveInstance(
            PlayerProgress.Instance.inspectedObjects,
            transform.position
        );

        if (audioSource.clip != null)
            Invoke(nameof(PlayRing), audioSource.clip.length);

        if (doorToUnlock != null)
            doorToUnlock.UnlockDoor();

        hasTriggered = true;
    }
    private void PlayRing()
    {
        waitingRoomTelephone.PlayRing();
    }
}