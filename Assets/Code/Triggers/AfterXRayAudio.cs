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
    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
                SubtitleManager.Instance.Show(soSubtitle);
            }

            PlayerProgress.Instance.RegisterInspection(idToGive);

            Invoke(nameof(PlayRing), audioSource.clip.length);


            if (doorToUnlock != null)
                doorToUnlock.UnlockDoor();  // 🔓 desbloquear puerta

            hasTriggered = true;
        }

    }
    private void PlayRing()
    {
        waitingRoomTelephone.PlayRing();
    }
}