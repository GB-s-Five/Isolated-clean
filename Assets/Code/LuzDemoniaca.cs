using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LuzDemoniaca : MonoBehaviour
{
    [Header("=== OJOS MALVADAS ===")]
    public Transform[] ojosMalvados; // Arrastra aquí los ojos (GameObjects o solo sus Transform)

    [Header("=== ZONA DE INFLUENCIA (AJUSTA A TU GUSTO) ===")]
    [Range(3f, 20f)] public float radioMaximo = 8f;   // 6-10 es perfecto para zonas pequeñas/medias

    [Header("=== VOLUMEN GLOBAL (Post-Procesado) ===")]
    public Volume volumeGlobal;

    [Header("=== PROGRESIÓN ULTRA SUAVE ===")]
    [Range(0.5f, 4f)] public float velocidadMaxima = 2f;     // Cuando estás a 0 metros
    [Range(0.1f, 2f)] public float velocidadBajada = 0.7f;   // Baja lentamente al salir
    public float locuraMinima = 0.03f;                        // 3% mínimo al entrar en el borde

    [Header("=== DAÑO LENTO ===")]
    public float danoMaxPorSegundo = 5f;
    public float saludMaxima = 100f;
    private float saludActual;

    // Efectos URP
    private Vignette vignette;
    private DepthOfField dof;
    private Bloom bloom;
    private ColorAdjustments color;
    private ChromaticAberration chromatic;
    private LensDistortion distortion;

    private float locura = 0f;

    private void Start()
    {
        saludActual = saludMaxima;

        if (volumeGlobal && volumeGlobal.profile)
        {
            volumeGlobal.profile.TryGet(out vignette); if (vignette) vignette.active = true;
            volumeGlobal.profile.TryGet(out dof); if (dof) { dof.active = true; dof.mode.value = DepthOfFieldMode.Gaussian; }
            volumeGlobal.profile.TryGet(out bloom); if (bloom) bloom.active = true;
            volumeGlobal.profile.TryGet(out color); if (color) color.active = true;
            volumeGlobal.profile.TryGet(out chromatic); if (chromatic) chromatic.active = true;
            volumeGlobal.profile.TryGet(out distortion); if (distortion) distortion.active = true;
        }
    }

    private void Update()
    {
        float influencia = 0f;
        float distanciaMinima = radioMaximo;

        // Busca el ojo más cercano
        foreach (Transform ojo in ojosMalvados)
        {
            if (ojo == null) continue;
            float dist = Vector3.Distance(transform.position, ojo.position);
            if (dist < distanciaMinima) distanciaMinima = dist;
        }

        // Calcula influencia progresiva (más fuerte cerca del ojo)
        if (distanciaMinima <= radioMaximo)
        {
            float ratio = 1f - (distanciaMinima / radioMaximo);
            influencia = Mathf.Pow(ratio, 0.5f); // Curva natural: lento al principio, más fuerte al final
        }

        // Subir o bajar locura
        if (influencia > 0.01f)
        {
            locura += velocidadMaxima * influencia * Time.deltaTime;
            if (locura < locuraMinima) locura = locuraMinima;
        }
        else
        {
            locura -= velocidadBajada * Time.deltaTime;
        }

        locura = Mathf.Clamp01(locura);

        AplicarEfectos(locura);
        AplicarDaño(locura);
    }

    void AplicarEfectos(float nivel)
    {
        float t = nivel;

        if (vignette) vignette.intensity.value = Mathf.Lerp(0f, 0.45f, t);
        if (dof)
        {
            dof.gaussianStart.value = Mathf.Lerp(3f, 1.5f, t);
            dof.gaussianEnd.value = Mathf.Lerp(15f, 6f, t);
        }
        if (bloom)
        {
            bloom.intensity.value = Mathf.Lerp(0.5f, 3f, t);
        }
        if (color)
        {
            color.saturation.value = Mathf.Lerp(0f, 25f, t);
            color.contrast.value = Mathf.Lerp(0f, 15f, t);
            color.colorFilter.value = Color.Lerp(Color.white, new Color(1f, 0.6f, 0.6f), t);
        }
        if (chromatic) chromatic.intensity.value = Mathf.Lerp(0f, 0.35f, t);
        if (distortion) distortion.intensity.value = Mathf.Lerp(0f, -20f, t);
    }

    void AplicarDaño(float nivel)
    {
        if (nivel > 0.1f)
        {
            float dano = danoMaxPorSegundo * nivel * nivel * Time.deltaTime;
            saludActual -= dano;

            if (saludActual <= 0f)
            {
                saludActual = 0f;
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("...has sido consumido lentamente por la presencia demoníaca");
            }
        }
    }

    // HUD discreto y elegante
    void OnGUI()
    {
        GUI.color = locura > 0.4f ? new Color(1f, 0.3f, 0.3f, 0.9f) : new Color(0.9f, 0.7f, 0.3f, 0.8f);
        GUI.skin.label.fontSize = 18;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(20, 20, 600, 30), "Despertarse: " + (locura * 100f).ToString("F0") + "%");
        GUI.Label(new Rect(20, 45, 600, 30), "Vida: " + saludActual.ToString("F0"));
    }

    // Gizmo para ver el radio en Scene View
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
        foreach (Transform ojo in ojosMalvados)
        {
            if (ojo != null)
                Gizmos.DrawSphere(ojo.position, radioMaximo);
        }
    }
}