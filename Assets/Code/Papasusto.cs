using System.Collections.Generic;
using UnityEngine;

public class Papasusto : MonoBehaviour
{
    [SerializeField] private GameObject father;

    
    [SerializeField] private GameObject tPosePrefab;
    public AudioSource tension;
    public AudioSource bum;
    [SerializeField] public Light[] Luces;

    [SerializeField] private string eventID = "Papasusto Realizado";    //id

    public void Start() //al start si la id es true 
    {
        if (PlayerProgress.Instance != null && PlayerProgress.Instance.HasInspected(eventID))
        {
            this.enabled = false;   //desactiva el codigo
        }
    }

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
        Invoke(nameof(PlayBum), 2f);
        //tension.Play();
    }

    private void PlayBum()
    {

        bum.Play();
    }
    
    private void Switch()
    {
        if (father != null)
        {
            
            Vector3 lastPosition = father.transform.position;
            Quaternion lastRotation = father.transform.rotation;

           
            Destroy(father);

           
            father = Instantiate(tPosePrefab, lastPosition, lastRotation);

            //guarda id player
            if (PlayerProgress.Instance != null)
            {
                PlayerProgress.Instance.RegisterInspection(eventID);
            }
            this.enabled = false;   //desactiva el codigo
        }
    }
}