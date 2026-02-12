using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EscController : MonoBehaviour
{
    public GameObject MenuESC;
    public AudioSource tension; // música de tensión
    private bool isPaused = false;
    public static EscController instance;
    [SerializeField] private string menu = "InitJuego";

    private List<AudioSource> pausedAudioSources = new List<AudioSource>();
    private float originalFixedDeltaTime;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
        originalFixedDeltaTime = Time.fixedDeltaTime; // guardamos valor original
        if (MenuESC != null) MenuESC.SetActive(false);

        if (tension != null)
            tension.ignoreListenerPause = true; // tensión no se pausa
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;

        // Pausar tiempo
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        tension.Play();
        // Pausar todos los AudioSources activos excepto la música de tensión
        pausedAudioSources.Clear();
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in allAudioSources)
        {
            if (a == tension) continue;
            if (a.isPlaying)
            {
                a.Pause();
                pausedAudioSources.Add(a); // guardamos los que estaban reproduciéndose
            }
        }

        // Cursor y menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (MenuESC != null)
            MenuESC.SetActive(true);

        Debug.Log("Juego pausado. AudioSources pausados: " + pausedAudioSources.Count);
    }

    public void Resume()
    {
        isPaused = false;
        tension.Stop();
        // Restaurar tiempo
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        // Reanudar todos los AudioSources que estaban reproduciéndose
        foreach (AudioSource a in pausedAudioSources)
        {
            if (a != null)
                a.UnPause();
        }

        // Limpiar la lista
        pausedAudioSources.Clear();

        // Cursor y menú
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (MenuESC != null)
            MenuESC.SetActive(false);

        Debug.Log("Juego reanudado. AudioSources reanudados.");
    }

    public void IraMenu()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        SceneManager.LoadScene(menu);

    }
}