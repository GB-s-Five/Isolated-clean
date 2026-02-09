using UnityEngine;

public class TensionAudioPasillo2 : MonoBehaviour
{
    AudioSource audioTension;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        audioTension = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;


        audioTension.Play();
    }
}
