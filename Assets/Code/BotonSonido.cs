using UnityEngine;

public class BotonSonido : MonoBehaviour
{
    public AudioSource source; 

    public void ReproducirSonido()
    {
        if (source != null)
        {
            source.Play();
        }
    }
}
