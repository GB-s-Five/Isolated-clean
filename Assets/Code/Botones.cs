using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Map";
    [SerializeField] private string creditos = "Creditos";

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
