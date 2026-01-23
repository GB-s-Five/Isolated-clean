using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }

    private Coroutine currentCoroutine;
    private SOSubtitle subtitles;
    [SerializeField] private TextMeshProUGUI subtitleText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple SubtitleManager instances found. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Show(SOSubtitle newSubtitle)
    {
        if (newSubtitle == null) return;
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ShowCoroutine(newSubtitle));
    }

    private IEnumerator ShowCoroutine(SOSubtitle newSubtitle)
    {
        subtitleText.gameObject.SetActive(true);
        for (int i=0; i<newSubtitle.messageFragments.Length; i++)
        {
            subtitleText.text = newSubtitle.messageFragments[i].phrase;
            yield return new WaitForSeconds(newSubtitle.messageFragments[i].duration);
        }
       

        subtitleText.gameObject.SetActive(false);
        subtitleText.text = "";
        currentCoroutine = null;
    }
}
