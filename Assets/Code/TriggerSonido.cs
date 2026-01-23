using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TriggerSonido : MonoBehaviour
{
    [Header("Configuración del sonido")]
    public SOSubtitle soSubtitle;               // Aquí arrastras el sonido que quieras
    public bool reproducirUnaVez = true; // Si quieres que solo suene la primera vez
    public bool fadeIn = false;          // Fade suave al entrar (opcional)
    public float tiempoFade = 1f;
    public float volumen = 1f;

    private AudioSource fuente;
    private bool yaSonó = false;

    private void Awake()
    {
        fuente = GetComponent<AudioSource>();
        if(soSubtitle == null) Debug.LogWarning("soSubtitle es nulo en TriggerSonido:"+ gameObject.ToString());
        else
        {
            if (soSubtitle.audioClip != null)
                fuente.clip = soSubtitle.audioClip;
            else
                Debug.LogWarning("Audioclip es nulo en TriggerSonido:"+ gameObject.ToString());
        }
        fuente.playOnAwake = false;
        fuente.volume = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (reproducirUnaVez && yaSonó) return;

        yaSonó = true;
        fuente.volume = 0f;
        fuente.Play();
        SubtitleManager.Instance.Show(soSubtitle);
        if (fadeIn)
            StartCoroutine(FadeIn());
        else
            fuente.volume = volumen;
    }

    
    
    System.Collections.IEnumerator FadeIn()
    {
        float t = 0;
        while (t < tiempoFade)
        {
            t += Time.deltaTime;
            fuente.volume = Mathf.Lerp(0f, volumen, t / tiempoFade);
            yield return null;
        }
        fuente.volume = volumen;
    }

    // Si quieres que se pueda volver a activar al salir (para pasillos repetibles)
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!reproducirUnaVez) yaSonó = false;
    }
    
}