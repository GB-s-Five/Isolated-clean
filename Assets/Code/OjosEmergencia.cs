using System.Collections;
using UnityEngine;

public class OjosEmergencia : MonoBehaviour
{
    [Header("Asigna estos en el Inspector")]
    public Light ojoIzquierdo;
    public Light ojoDerecho;
    public AudioSource audioDelSusto;

    private bool yaTerminoElAudio = false;
    private bool estaParpadeando = false;

    void Start()
    {
        // Los ojos empiezan encendidos y rojos (por si acaso)
        ojoIzquierdo.color = Color.red;
        ojoDerecho.color = Color.red;
        ojoIzquierdo.enabled = true;
        ojoDerecho.enabled = true;

        // Reproduce el audio al empezar
        if (audioDelSusto != null)
            audioDelSusto.Play();
    }

    void Update()
    {
        // Cuando termina el audio → empieza el parpadeo de emergencia
        if (!yaTerminoElAudio && !audioDelSusto.isPlaying && audioDelSusto.time > 0)
        {
            yaTerminoElAudio = true;
            StartCoroutine(ParpadeoEmergencia());
        }
    }

    IEnumerator ParpadeoEmergencia()
    {
        estaParpadeando = true;

        while (estaParpadeando)
        {
            // Ojo izquierdo ON, derecho OFF
            ojoIzquierdo.enabled = true;
            ojoDerecho.enabled = false;
            yield return new WaitForSeconds(0.25f);

            // Ojo izquierdo OFF, derecho ON
            ojoIzquierdo.enabled = false;
            ojoDerecho.enabled = true;
            yield return new WaitForSeconds(0.25f);
        }

        // Cuando termine el bucle, asegura que estén apagados
        ojoIzquierdo.enabled = false;
        ojoDerecho.enabled = false;
    }

    // Cuando el jugador entra en la zona de trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaParpadeando = false; // Para el parpadeo
            ojoIzquierdo.enabled = false;
            ojoDerecho.enabled = false;
        }
    }
}