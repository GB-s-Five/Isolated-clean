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

    IEnumerator FinVideo()
    {
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        SceneManager.LoadScene(Menu);
    }
}
