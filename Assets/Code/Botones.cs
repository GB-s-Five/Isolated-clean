using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Map";
    [SerializeField] private string creditos = "Creditos";
    public AudioSource cajita;
    public AudioSource loop;

    public void Awake()
    {
        cajita.Play();
        Invoke(nameof(Playloop), 3f);
    }

    private void Playloop()
    {
        loop.Play();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void LoadCreditos()
    {
        SceneManager.LoadScene(creditos);
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
