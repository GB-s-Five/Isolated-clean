using UnityEngine;
using System.Collections;

public class SalidaActivaMesaMovimiento : MonoBehaviour
{
    [Header("ID que debe tener inspeccionado el jugador")]
    [SerializeField] private string requiredInspectionID = "Rayos";

    [Header("Referencia a la mesa que se va a mover")]
    [SerializeField] private Transform mesa;

    [Header("Movimiento")]
    [SerializeField] private Vector3 desplazamiento = new Vector3(0, -1f, 0);
    [SerializeField] private float duracion = 1.2f;

    [Header("Clip jumpscare (arrastrar AudioClip)")]
    [SerializeField] private AudioClip jumpscareClip;  // ← Aquí arrastras un CLIP de audio

    [Header("Clip  (arrastrar AudioClip)")]
    [SerializeField] private AudioClip RingAudio;  // ← Aquí arrastras un CLIP de audio

    private AudioSource audioSource;
    private bool yaSeMovio = false;

    private void Awake()
    {
        // Creamos un AudioSource automáticamente si no existe
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (yaSeMovio) return;
        if (!other.CompareTag("Player")) return;

        if (PlayerProgress.Instance.HasInspected(requiredInspectionID))
        {
            yaSeMovio = true;
            Debug.Log("Movimiento de mesa activado por inspección: " + requiredInspectionID);
            StartCoroutine(MoverMesa());
            //Destroy(gameObject);
        }
    }

    private IEnumerator MoverMesa()
    {
        Vector3 inicio = mesa.position;
        Vector3 destino = inicio + desplazamiento;

        float t = 0f;

        if (jumpscareClip != null)
            audioSource.PlayOneShot(jumpscareClip);

        while (t < duracion)
        {
            t += Time.deltaTime;
            mesa.position = Vector3.Lerp(inicio, destino, t / duracion);
            yield return null;
        }

        mesa.position = destino;

 
    }
}
