using UnityEngine;
using System.Collections;

public class SalidaActivaMesaMovimiento : MonoBehaviour
{
    [Header("ID que debe tener inspeccionado el jugador")]
    [SerializeField] private string requiredInspectionID = "Rayos";

    [Header("Referencia a la mesa que se va a mover")]
    [SerializeField] private Transform mesa;

    [Header("Movimiento")]
    [SerializeField] private Vector3 desplazamiento = new Vector3(-1f, 0, 0);
    [SerializeField] private float duracion = 3f;

    [Header("Clip jumpscare (arrastrar AudioClip)")]
    [SerializeField] private AudioClip jumpscareClip;  // ← Aquí arrastras un CLIP de audio

    [Header("Clip  (arrastrar AudioClip)")]
    [SerializeField] private AudioClip RingAudio;  // ← Aquí arrastras un CLIP de audio

    private AudioSource audioSource;
    private bool yaSeMovio = false;
    [SerializeField] private float velocidad;

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
            //Invoke(nameof(MoverMesa),0f); // Pequeña demora antes de mover la mesa
            //audioSource.PlayOneShot(RingAudio);
            //Destroy(gameObject);
        }
    }

    private  IEnumerator MoverMesa()
    {
        Vector3 inicio = mesa.position;
        Vector3 destino = inicio + desplazamiento;
        float distancia =  destino.x - inicio.x;
        float t = 0f;
        Debug.Log("Distancia = " + distancia);
        if (jumpscareClip != null)
            audioSource.PlayOneShot(jumpscareClip);

        while (Mathf.Abs(distancia) > 1f)
        {
            //t += Time.deltaTime;
            Debug.Log("t = " + t);
            mesa.position -= new Vector3 (velocidad,0,0);
            yield return new WaitForSeconds(0.005f);
            
        }

        //mesa.position = destino;

 
    }
}
