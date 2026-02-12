using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorSlapPatientRoom : MonoBehaviour
{
    [Header("Inspectable Object ID Required")]
    [SerializeField] private string requiredInspectionID = "LightMessage";
    [SerializeField] private string givedID = "doorSlapPatientRoom";
    [Header("Audio")]
    [SerializeField] private AudioSource doorSlapAudio;

    [Header("Puerta")]
    [SerializeField] private Transform puerta; // La puerta que se cierra
    [SerializeField] private float velocidadCierre = 0.5f; // Tiempo de cierre (portazo)
    [SerializeField] private Vector3 rotacionCerradaEuler; // ROTACIÓN CERRADA

    private bool hasTriggered = false;
    

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerProgress.Instance.HasInspected(givedID)) return;
        if (!other.CompareTag("Player")) return;

        if (!PlayerProgress.Instance.HasInspected(requiredInspectionID))
            return;

        // Sonido del portazo
        if (doorSlapAudio != null)
        {
            doorSlapAudio.Play();
            Debug.Log("¡Portazo reproducido!");
        }
        else
        {
            Debug.LogWarning("No se asignó AudioSource para el portazo.");
        }

        // Cierre de la puerta
        if (puerta != null)
        {
            StartCoroutine(CerrarPuerta());
        }
        else
        {
            Debug.LogWarning("No se asignó la puerta en el campo 'Puerta'.");
        }

        hasTriggered = true;
        PlayerProgress.Instance.RegisterInspection(givedID);
        // Checkpoint
        if (Checkpointmanager.Instance != null)
        {
            Debug.Log("Checkpoint alcanzado: " +String.Join("/",PlayerProgress.Instance.inspectedObjects));
            Checkpointmanager.Instance.SaveInstance(
                PlayerProgress.Instance.inspectedObjects,
                transform.position
            );
        }
        
    }

    private System.Collections.IEnumerator CerrarPuerta()
    {
        Quaternion rotacionInicial = puerta.localRotation;
        Quaternion rotacionCerrada = Quaternion.Euler(rotacionCerradaEuler);
        float tiempo = 0f;

        while (tiempo < velocidadCierre)
        {
            tiempo += Time.deltaTime;
            puerta.localRotation = Quaternion.Lerp(
                rotacionInicial,
                rotacionCerrada,
                tiempo / velocidadCierre
            );
            yield return null;
        }

        puerta.localRotation = rotacionCerrada;
        Debug.Log("Puerta cerrada con portazo!");
    }
}
