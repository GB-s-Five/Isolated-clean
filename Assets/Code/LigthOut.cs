using UnityEngine;

public class LightOut : MonoBehaviour
{
    [SerializeField] private DoorController door;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && door.isOpen)
        {
           // Debug.Log("Comparacion correcta condiciones para apagar la luz correctas");
            Invoke(nameof(DisableObject), 3f);
        } //else Debug.Log("No se cumplen condiciones");
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
       // Debug.Log("LUZ APAGADA");
    }
}