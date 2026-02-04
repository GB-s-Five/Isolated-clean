using System.Collections.Generic;
using UnityEngine;

public class Papasusto : MonoBehaviour
{
    [SerializeField] private GameObject father;

    
    [SerializeField] private GameObject tPosePrefab;

    [SerializeField] public Light[] Luces;

    public void ToggleLights()
    {
        foreach (Light luz in Luces)
        {
            if (luz.type == LightType.Point)
            {
                luz.enabled = !luz.enabled;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ToggleLights();
        Invoke(nameof(Switch), 1.5f);
        Invoke(nameof(ToggleLights), 2f);
    }

    private void Switch()
    {
        if (father != null)
        {
            
            Vector3 lastPosition = father.transform.position;
            Quaternion lastRotation = father.transform.rotation;

           
            Destroy(father);

           
            father = Instantiate(tPosePrefab, lastPosition, lastRotation);
        }
    }
}