using UnityEngine;

public class ActivarAlarma : MonoBehaviour
{
    [Header("=== ALARMA SONIDO ===")]
    public AudioClip sonidoAlarma;
    public float volumenAlarma = 0.8f;

    [Header("=== LUZ DE ALARMA ===")]
    public Light luzAlarma;
    public Color colorAlarma = new Color(1f, 0f, 0f);
    public float intensidadMax = 15f;
    public float velocidadParpadeo = 3f;

    [Header("=== INTERACCIÓN ===")]
    public KeyCode teclaInteractuar = KeyCode.E;
    public float distanciaMaxima = 3f;
    public string textoInteractuar = "Pulsa E para activar";

    private AudioSource fuenteAlarma;
    private bool alarmaActivada = false;
    private float timerParpadeo = 0f;

   

    private void Start()
    {
        // Crear AudioSource para alarma
        fuenteAlarma = gameObject.AddComponent<AudioSource>();
        if (sonidoAlarma != null)
        {
            fuenteAlarma.clip = sonidoAlarma;
            fuenteAlarma.loop = true;
        }
        fuenteAlarma.volume = 0f;
        fuenteAlarma.playOnAwake = false;

        // Apagar luz
        if (luzAlarma) luzAlarma.enabled = false;
    }

    private void Update()
    {
        // Texto interactivo
        if (PuedeInteractuar() && !alarmaActivada)
            MostrarTextoInteractivo(textoInteractuar);

        // Pulsar E
        if (PuedeInteractuar() && Input.GetKeyDown(teclaInteractuar) && !alarmaActivada)
            ActivarLaAlarma();

        // Parpadeo
        if (alarmaActivada && luzAlarma)
        {
            timerParpadeo += Time.deltaTime * velocidadParpadeo;
            float intensidad = intensidadMax * (0.5f + 0.5f * Mathf.Sin(timerParpadeo * Mathf.PI));
            luzAlarma.intensity = intensidad;
        }
    }

    bool PuedeInteractuar()
    {
        return Vector3.Distance(transform.position, Camera.main.transform.position) <= distanciaMaxima;
    }

    void ActivarLaAlarma()
    {
        alarmaActivada = true;

        // SONIDO
        if (fuenteAlarma && sonidoAlarma)
        {
            fuenteAlarma.volume = volumenAlarma;
            fuenteAlarma.Play();
        }

        // LUZ ROJA
        if (luzAlarma)
        {
            luzAlarma.enabled = true;
            luzAlarma.color = colorAlarma;
        }

        Debug.Log("🚨 ¡ALARMA ACTIVADA!");
    }

    void MostrarTextoInteractivo(string texto)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
        if (screenPos.z > 0)
        {
            GUI.color = Color.red;
            GUI.skin.label.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(screenPos.x - 100, Screen.height - screenPos.y - 20, 200, 40), texto);
        }
    }
}