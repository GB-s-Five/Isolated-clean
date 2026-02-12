using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;


public class TrampaTriggerMurmullos : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private DoorController doorController;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip murmurosos;
    [SerializeField] private AudioClip sonidoPuerta;

    [SerializeField] private HeadBobSystem headBobSystem;
    [SerializeField] private FootstepsController footstepsController;
    [SerializeField] private CandleTurnOffLights candlesController;
    private void Awake()
    {
        audioSource.clip = murmurosos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        audioSource.clip = null;
        congelarJugador();
    }

    void congelarJugador()
    {
               
        if (footstepsController != null) {
            footstepsController.enabled = false; //bloquea el movimiento del jugador
            Debug.Log($"{name}: Bloquea el movimiento");
        }

        if (headBobSystem != null) {
            headBobSystem.enabled = false; //bloquea el movimiento del jugador
            Debug.Log($"{name}: Bloquea el movimiento");
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false; //bloquea el movimiento del jugador
            cerrarPuerta();
        }
    }

    void cerrarPuerta()
    {
        if (doorController.isOpen)
        {
            doorController.tiempo = 3f;
            doorController.Toggle();

            audioSource.clip = sonidoPuerta;
            StartCoroutine(Silencio());
        }
        else
        {
            candlesController.LightsOffFinalEventStart();
        }
    }

    IEnumerator Silencio()
    {
        yield return new WaitForSeconds(3f);
        audioSource.clip = null;

        candlesController.LightsOffFinalEventStart();
    }
}
