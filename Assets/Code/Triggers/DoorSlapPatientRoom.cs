using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorSlapPatientRoom : MonoBehaviour
{
    [Header("Inspectable Object ID Required")]
    [SerializeField] private string requiredInspectionID = "LightMessage";

    [Header("Audio")]
    [SerializeField] private AudioSource doorSlapAudio;
    
    [Header("Puerta")]
    [SerializeField] private Transform puerta; // La puerta que se cierra
    [SerializeField] private float velocidadCierre = 0.5f; // Tiempo de cierre (rápido para portazo)

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (other.CompareTag("Player"))
        {
            if (PlayerProgress.Instance.HasInspected(requiredInspectionID))
            {
                if (doorSlapAudio != null)
                {
                    doorSlapAudio.Play();
                    Debug.Log("¡Portazo reproducido!");
                }
                else
                {
                    Debug.LogWarning("No se asignó AudioSource para el portazo.");
                }

                if (puerta != null)
                {
                    StartCoroutine(CerrarPuerta());
                }
                else
                {
                    Debug.LogWarning("No se asignó la puerta en el campo 'Puerta'.");
                }
                
                hasTriggered = true;
                //Checkpoint
                Debug.Log("Checkpoint alcanzado");
                Checkpointmanager.Instance.SaveInstance(PlayerProgress.Instance.inspectedObjects,this.transform.position);
            }
        }
    }

    private System.Collections.IEnumerator CerrarPuerta()
    {
        Quaternion rotacionInicial = puerta.localRotation;
        Quaternion rotacionCerrada = Quaternion.identity; // Posición cerrada (0°)
        float tiempo = 0f;

        while (tiempo < velocidadCierre)
        {
            tiempo += Time.deltaTime;
            puerta.localRotation = Quaternion.Lerp(rotacionInicial, rotacionCerrada, tiempo / velocidadCierre);
            yield return null;
        }

        puerta.localRotation = rotacionCerrada; // Asegura posición final
        Debug.Log("Puerta cerrada con portazo!");
    }
}