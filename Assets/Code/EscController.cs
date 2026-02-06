using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EscController : MonoBehaviour
{
    public GameObject MenuESC;
    private bool isPaused = false;
    public static EscController instance;
    public AudioSource tension;
    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
        if (MenuESC != null) MenuESC.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    public void Pause()
    {
        isPaused = !isPaused;
        if (tension.isPlaying)
        {
            tension.Stop();
        }
        else
        {
            tension.Play();
        }
            
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (MenuESC != null)
            MenuESC.SetActive(isPaused);

        
        Debug.Log("pausa= " + isPaused + "time=" + Time.timeScale);
    }
    public void Resume()
    {
        tension.Stop();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map");
        if (MenuESC != null) MenuESC.SetActive(false);
    }
    public void QuitGame()
{
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
}
}