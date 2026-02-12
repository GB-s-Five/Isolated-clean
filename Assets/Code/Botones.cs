using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Map";
    [SerializeField] private string creditos = "Creditos";
    public AudioSource cajita;
    public AudioSource loop;
    public AudioClip loopClip;

    public void OnEnable()
    {
        //cajita.Play();
        //Invoke(nameof(Playloop), 3f);
        StartCoroutine("PlaySounds");
    }
    IEnumerator PlaySounds()
    {
        cajita.Play();
        yield return new WaitForSeconds(4);
        loop.Play();

    }
    private void Playloop()
    {
        Debug.Log("He entrado aqui");
        loop.PlayOneShot(loopClip);
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
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}