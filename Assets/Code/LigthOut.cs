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
            Invoke(nameof(DisableObject), 3f);
        }
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
                child.GetComponent<LigthTrigger>().enabled = false;
            }
        }
    }
}