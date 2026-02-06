using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TriggerSonido : MonoBehaviour
{
    [Header("Configuraci�n del sonido")]
    public SOSubtitle soSubtitle;               // Aqu� arrastras el sonido que quieras
    public bool fadeIn = false;          // Fade suave al entrar (opcional)
    public float tiempoFade = 1f;
    public float volumen = 1f;
    public List<String> requiredIDs;
    public string endID = "";
    private AudioSource fuente;
    private bool canTrigger = true;
    
    

    private void Awake()
    {
        fuente = GetComponent<AudioSource>();
        if(soSubtitle == null) Debug.LogWarning("soSubtitle es nulo en TriggerSonido:"+ gameObject.ToString());
        else
        {
            if (soSubtitle.audioClip != null)
                fuente.clip = soSubtitle.audioClip;
            else
                Debug.LogWarning("Audioclip es nulo en TriggerSonido:"+ gameObject.ToString());
        }
        fuente.playOnAwake = false;
        fuente.volume = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        canTrigger = true;
        if (!other.CompareTag("Player")) return;
        if (PlayerProgress.Instance.HasInspected(endID)) return;
        foreach (String iD in requiredIDs)
            if (!PlayerProgress.Instance.HasInspected(iD))
            {
                canTrigger = false;
                break;
            }
        if (canTrigger) {
            PlaySound();
        }
    }

    public void PlaySound()
    {
        fuente.volume = 0f;
        fuente.Play();
        SubtitleManager.Instance.Show(soSubtitle);
        if (fadeIn)
            StartCoroutine(FadeIn());
        else
            fuente.volume = volumen;
        if (endID != "") PlayerProgress.Instance.RegisterInspection(endID);
    }
    
    System.Collections.IEnumerator FadeIn()
    {
        float t = 0;
        while (t < tiempoFade)
        {
            t += Time.deltaTime;
            fuente.volume = Mathf.Lerp(0f, volumen, t / tiempoFade);
            yield return null;
        }
        fuente.volume = volumen;
    }
}