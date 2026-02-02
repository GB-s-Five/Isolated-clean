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
            //Debug.Log("Comparacion correcta condiciones para apagar la luz correctas");
            Invoke(nameof(DisableObject), 3f);
        } //else Debug.Log("No se cumplen condiciones");
    }

    void DisableObject()
    {
        gameObject.SetActive(false);

        //Obtener el padre del gameObject actual
        Transform parentTransform = transform.parent;

        //Desactivar los hijos del padre que tengan un Light component
        foreach (Transform child in parentTransform)
        {
            Light lightComponent = child.GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.enabled = false;
            }
        }


        //Debug.Log("LUZ APAGADA");
    }
}