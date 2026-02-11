using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer; // Asignar en inspector
    private float previousVolume;

    public void PauseAllAudio()
    {
        // Guardamos el volumen actual
        masterMixer.GetFloat("Volume", out previousVolume);
        // Ponemos volumen a -80 dB (silencio)
        masterMixer.SetFloat("Volume", -80f);
    }

    public void ResumeAllAudio()
    {
        // Restauramos el volumen
        masterMixer.SetFloat("Volume", previousVolume);
    }
}
