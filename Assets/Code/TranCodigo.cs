using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Video;

public class TranCodigo : MonoBehaviour
{
    [SerializeField] private string Menu = "InitJuego";
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        StartCoroutine(FinVideo());
    }

    private void Update()   //esc vuelve al menu
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(Menu);
        }
    }
    IEnumerator FinVideo()
    {
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        SceneManager.LoadScene(Menu);
    }
}
