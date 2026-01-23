// SCRIPT 2: ZonaSegura.cs (GAMEOBJECT VACÍO HIJO - JUMPSCARE)
using UnityEngine;

public class ZonaSegura : MonoBehaviour
{
    [Header("=== JUMPSCARE ===")]
    public AudioClip jumpscareClip;
    public float volumenJumpscare = 1f;

    [Header("=== REFERENCIA ===")]
    public TriggerAudioThenAlarma scriptPrincipal;  // Arrastra el objeto PADRE

    private AudioSource fuenteJumpscare;
    private bool yaSonóJumpscare = false;

    private void Awake()
    {
        // CREAR AUDIOSOURCE AUTOMÁTICO
        fuenteJumpscare = gameObject.AddComponent<AudioSource>();
        fuenteJumpscare.playOnAwake = false;
        fuenteJumpscare.loop = false;
        fuenteJumpscare.spatialBlend = 0f;  // 2D fuerte
        fuenteJumpscare.volume = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || yaSonóJumpscare) return;

        yaSonóJumpscare = true;

        // 1. REPRODUCIR JUMPSCARE
        if (jumpscareClip != null)
        {
            fuenteJumpscare.clip = jumpscareClip;
            fuenteJumpscare.volume = volumenJumpscare;
            fuenteJumpscare.Play();
            Debug.Log("🎃 ¡JUMPSCARE REPRODUCIDO!");
        }

        // 2. LLAMAR AL SCRIPT PRINCIPAL PARA APAGAR TODO
        if (scriptPrincipal != null)
            scriptPrincipal.JugadorEntroZonaSegura();
    }
}