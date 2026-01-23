using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class SoundTrigger : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // si no se asigna, intentar� GetComponent
    [SerializeField] private SOSubtitle soSubtitle;
    [Header("Subt�tulos fallback (si no hay SubtitleManager)")]
    [SerializeField] private TextMeshProUGUI subtitleTextFallback; // opcional
    [TextArea][SerializeField] private string subtitleLine;
    [SerializeField] private bool playOnce = true;

    private bool hasPlayed = false;
    private Coroutine localSubtitleCoroutine;

    private void Reset()
    {
        // Para asegurar que el collider sea trigger por defecto al a�adir el script
        Collider c = GetComponent<Collider>();
        if (c != null) c.isTrigger = true;
    }

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning($"SoundTrigger on '{name}' has no AudioSource assigned or attached.");
        }

        // Aseg�rate de que el collider sea trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null && !collider.isTrigger)
        {
            Debug.LogWarning($"Collider on '{name}' is not set as IsTrigger. Setting it automatically.");
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            // no es el jugador, ignorar
            return;
        }

        if (playOnce && hasPlayed)
        {
            Debug.Log($"SoundTrigger '{name}': ya reproducido (playOnce).");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError($"SoundTrigger '{name}': No AudioSource disponible. Asigna un AudioSource con un clip.");
            return;
        }

        if (audioSource.clip == null)
        {
            Debug.LogError($"SoundTrigger '{name}': AudioSource_clip es null. Asigna un AudioClip al AudioSource.");
            return;
        }

        Debug.Log($"SoundTrigger '{name}': Player entr�, reproduciendo audio '{audioSource.clip.name}'.");

        audioSource.Play();

        // Prefiere SubtitleManager singleton si existe
        if (SubtitleManager.Instance != null)
        {
            SubtitleManager.Instance.Show(soSubtitle);
            Debug.Log("Usando SubtitleManager.Instance para mostrar subt�tulo.");
        }
        else if (subtitleTextFallback != null)
        {
            // fallback local si no hay SubtitleManager
            if (localSubtitleCoroutine != null) StopCoroutine(localSubtitleCoroutine);
            localSubtitleCoroutine = StartCoroutine(LocalShowSubtitle(subtitleLine, audioSource.clip.length));
            Debug.Log("Usando subtitleTextFallback para mostrar subt�tulo.");
        }
        else
        {
            Debug.Log("No hay SubtitleManager ni subtitleTextFallback asignado. No se muestran subt�tulos.");
        }

        hasPlayed = true;
    }

    private IEnumerator LocalShowSubtitle(string text, float duration)
    {
        if (subtitleTextFallback == null) yield break;

        subtitleTextFallback.text = text;
        subtitleTextFallback.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        subtitleTextFallback.gameObject.SetActive(false);
        subtitleTextFallback.text = "";
        localSubtitleCoroutine = null;
    }
}
