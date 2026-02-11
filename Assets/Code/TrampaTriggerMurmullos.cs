using UnityEngine;

public class TrampaTriggerMurmullos : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        congelarJugador();
        cerrarPuerta();

        GetComponent<AudioSource>().enabled = false;
    }

    void congelarJugador()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false; //bloquea el movimiento del jugador
            Debug.Log($"{name}: Bloquea el movimiento");
        }
    }

    void cerrarPuerta()
    {

    }
}
