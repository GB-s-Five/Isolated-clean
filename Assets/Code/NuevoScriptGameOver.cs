using UnityEngine;
using UnityEngine.SceneManagement;

public class NuevoSciptGameOver : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Map";
    [SerializeField] private string creditos = "Creditos";
    public AudioSource Malro;
    

    public void Awake()
    {
        Malro.Play();
        
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
