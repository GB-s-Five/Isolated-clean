using System.Collections.Generic;
using UnityEngine;

public class Papasusto : MonoBehaviour
{
    [SerializeField] private GameObject father;

    
    [SerializeField] private GameObject tPosePrefab;
    public AudioSource audio;
    public AudioSource audio2;
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
        Luces[0].GetComponent<FlickeringLight>().enabled = false;
    }

    public void ToggleFlicker()
    {
        Luces[0].GetComponent<FlickeringLight>().enabled = !Luces[0].GetComponent<FlickeringLight>().enabled;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ToggleLights();
        Invoke(nameof(Switch), 1.7f);
        Invoke(nameof(ToggleLights), 2f);
        Invoke(nameof(ToggleFlicker), 4);
        Invoke(nameof(PlayAudio2), 2f);
        audio.Play();
    }

    private void PlayAudio2()
    {

        audio2.Play();
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