using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EyeBlinkEffect : MonoBehaviour
{
    [Header("Banda de ojos")]
    public RectTransform topBand;
    public RectTransform bottomBand;

    [Header("Configuración del parpadeo")]
    public float blinkDuration = 0.8f;   // Duración de cada cierre/apertura
    public int oscillations = 3;         // Número de parpadeos completos
    public float pauseBetween = 0.05f;   // Pausa entre movimientos
    public float finalCloseOffset = 0f;  // Posición final de las bandas (0 = cerrado)
    [SerializeField] Animator eyesAnimator;
    [SerializeField] ScriptPlayerLive scriptPlayerLive;
    [SerializeField] private string gameOverScene = "GameOver";


    /// <summary>
    /// Inicia el parpadeo. Llama a onComplete al terminar.
    /// </summary>
    /// 
    void Start()
    {
        scriptPlayerLive.OnPlayerDeath+= DieHandler;
        scriptPlayerLive.OnPlayerDamaged+= DamageHandler;

    }

    private void DamageHandler()
    {
        eyesAnimator.SetTrigger("damage");
    }

    private void DieHandler()
    {
        eyesAnimator.SetBool("death",true);
    }
    public void GameOverScene()
    {
        
        SceneManager.LoadScene(gameOverScene);
    }

    public void Sound()
    {
        
    }
    // public void Blink(System.Action onComplete = null)
    // {
    //     StartCoroutine(BlinkCoroutine(onComplete));
    // }
    // private IEnumerator BlinkCoroutine(System.Action onComplete)
    // {
    //     Vector2 topOpen = topBand.anchoredPosition;
    //     Vector2 bottomOpen = bottomBand.anchoredPosition;

    //     Vector2 topClosed = new Vector2(topOpen.x, finalCloseOffset);
    //     Vector2 bottomClosed = new Vector2(bottomOpen.x, -finalCloseOffset);

    //     // Llamamos una sola vez, haciendo todas las oscilaciones
    //     yield return MoveBands(topBand, bottomBand, topOpen, bottomOpen, topClosed, bottomClosed, blinkDuration, oscillations);

    //     onComplete?.Invoke();
    // }

    // private IEnumerator MoveBands(RectTransform top, RectTransform bottom,
    //                             Vector2 topOpen, Vector2 bottomOpen,
    //                             Vector2 topClosed, Vector2 bottomClosed,
    //                             float duration, int oscillations)
    // {
    //     for (int i = 0; i < oscillations; i++)
    //     {
    //         // Cerrar desde la posición actual
    //         yield return SmoothMove(top, bottom, topClosed, bottomClosed, duration);
    //         // Abrir desde la posición actual
    //         yield return SmoothMove(top, bottom, topOpen, bottomOpen, duration);
    //     }

    //     // Opcional: dejar el ojo abierto al final
    //     yield return SmoothMove(top, bottom, topOpen, bottomOpen, duration);
    // }

    // // SmoothMove siempre parte de la posición actual de cada banda
    // private IEnumerator SmoothMove(RectTransform top, RectTransform bottom,
    //                             Vector2 targetTop, Vector2 targetBottom,
    //                             float duration)
    // {
    //     float elapsed = 0f;

    //     // La posición inicial se toma **al momento de iniciar cada movimiento**
    //     Vector2 startTop = top.anchoredPosition;
    //     Vector2 startBottom = bottom.anchoredPosition;

    //     while (elapsed < duration)
    //     {
    //         elapsed += Time.deltaTime;
    //         float t = Mathf.Clamp01(elapsed / duration);
    //         float smoothT = Mathf.SmoothStep(0f, 1f, t);

    //         top.anchoredPosition = Vector2.Lerp(startTop, targetTop, smoothT);
    //         bottom.anchoredPosition = Vector2.Lerp(startBottom, targetBottom, smoothT);

    //         yield return null;
    //     }

    //     // Aseguramos la posición final exacta
    //     top.anchoredPosition = targetTop;
    //     bottom.anchoredPosition = targetBottom;
    // }
}
