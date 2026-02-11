using System;
using UnityEngine;

public class LightOut : MonoBehaviour
{
    [SerializeField] private DoorController door;
    [SerializeField] private AudioSource sound;
    [SerializeField] private string idLuz;
    [SerializeField] private Light LuzApagar;

    private void Start()
    {
        if (PlayerProgress.Instance.HasInspected(idLuz))
        {
            DisableObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && door.isOpen && !PlayerProgress.Instance.HasInspected(idLuz))
        {
            Invoke(nameof(DisableObject), 3f);
        }
    }

    void DisableObject()
    {
        

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
        LuzApagar.enabled = false;
        if (!PlayerProgress.Instance.HasInspected(idLuz))
        {
            sound.Play();
            PlayerProgress.Instance.RegisterInspection(idLuz);
        }
        //gameObject.SetActive(false);
    }
}