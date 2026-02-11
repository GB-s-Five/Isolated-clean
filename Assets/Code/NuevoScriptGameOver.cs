using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NuevoSciptGameOver : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Map";
    [SerializeField] private string creditos = "Creditos";
    [SerializeField] private string menu = "InitJuego";
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
    public void IraMenu()
    {
        SceneManager.LoadScene(menu);
    }
}
