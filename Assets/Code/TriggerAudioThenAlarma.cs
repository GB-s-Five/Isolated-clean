// SCRIPT 1: TriggerAudioThenAlarma.cs (OBJETO PRINCIPAL - TRIGGER DE VOZ/ALARMA)
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public delegate void LightsEventHandler(bool stateLights);



[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]

public class TriggerAudioThenAlarma : MonoBehaviour
{
    [Header("=== AUDIO INICIAL (voz, susurro...) ===")]
    public AudioClip audioInicial;
    public float volumenInicial = 1f;

    [Header("=== ALARMA (2D - NO REPETIBLE) ===")]
    public AudioClip sonidoAlarma;
    public float volumenAlarma = 0.8f;

    [Header("=== LUCES DE ALARMA (parpadeantes) ===")]
    public Light[] lucesAlarma;
    [SerializeField] private float baseLightintensity = 8f;

    [Header("=== SPHERES OJOS DEMONÍACOS ===")]
    public GameObject[] spheresOjos;

    [Header("=== ZONA SEGURA (COLLIDER A DESACTIVAR) ===")]
    public Collider zonaSeguraCollider;  // Arrastra AQUÍ el BoxCollider del GameObject vacío

    [Header("Required Inspection ID")]
    [SerializeField] private string requiredInspectionID = "Rayos";

    public static event LightsEventHandler LightsEvent;


    private AudioSource fuente;
    private static bool yaActivadoGlobal = false;  // NUNCA se repite
    private bool infiernoActivado = false;
    private bool todoDesactivado = false;

    
    

    private void Awake()
    {
        fuente = GetComponent<AudioSource>();
        fuente.playOnAwake = false;
        fuente.spatialBlend = 0f;  // ALARMA 100% 2D
        ApagarTodo();

        // ZONA SEGURA OCULTA AL INICIO (solo desactivamos el collider)
        if (zonaSeguraCollider != null)
            zonaSeguraCollider.enabled = false;
    }

    public void ActivateAlarm()
    {
        if (yaActivadoGlobal) return;

        yaActivadoGlobal = true;

        // 2. ACTIVAR INFIERNO + ZONA SEGURA
        infiernoActivado = true;
        Debug.Log("🚨 ¡INFIERNO ACTIVADO!");

        // ACTIVAR COLLIDER DE ZONA SEGURA
        if (zonaSeguraCollider != null)
            zonaSeguraCollider.enabled = true;

        // ALARMA EN LOOP 2D
        fuente.clip = sonidoAlarma;
        fuente.loop = true;
        fuente.volume = volumenAlarma;
        fuente.spatialBlend = 0f;
        fuente.Play();

        ActivarLucesYEsferas();
    }

    // MÉTODO PÚBLICO que llama la zona segura
    public void JugadorEntroZonaSegura()
    {
        if (!infiernoActivado || todoDesactivado) return;
        DesaparecerTodoRepentinamente();
    }

    private void Update()
    {
        // PARPADEO LUCES
        if (infiernoActivado && !todoDesactivado && lucesAlarma != null)
        {
            float intensidad = 8f + baseLightintensity * Mathf.Sin(Time.time * 12f);
            foreach (Light luz in lucesAlarma)
                if (luz) luz.intensity = intensidad;
        }
    }

    void DesaparecerTodoRepentinamente()
    {
        LightsEvent?.Invoke(true);
        if (todoDesactivado) return;
        todoDesactivado = true;

        Debug.Log("💀 ¡ZONA SEGURA! TODO DESAPARECE DE GOLPE");

        // PARAR ALARMA
        if (fuente.isPlaying) fuente.Stop();

        // APAGAR LUCES
        foreach (Light luz in lucesAlarma)
            if (luz) luz.enabled = false;

        // APAGAR OJOS
        foreach (GameObject ojo in spheresOjos)
            if (ojo) ojo.SetActive(false);

    }

    void ActivarLucesYEsferas()
    {
        LightsEvent?.Invoke(false);

        foreach (Light luz in lucesAlarma)
            if (luz) { luz.enabled = true; luz.color = Color.red; }

        foreach (GameObject ojo in spheresOjos)
            if (ojo) ojo.SetActive(true);  
    }

    void ApagarTodo()
    {
        foreach (Light luz in lucesAlarma)
            if (luz) luz.enabled = false;

        foreach (GameObject ojo in spheresOjos)
            if (ojo) ojo.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        BoxCollider box = GetComponent<BoxCollider>();
        if (box) Gizmos.DrawWireCube(transform.position, box.size);
    }
}