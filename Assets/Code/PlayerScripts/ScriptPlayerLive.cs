using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class ScriptPlayerLive : MonoBehaviour
{
    public static ScriptPlayerLive Instance;

    public float live = 100f;                // Vida actual
    public float maxLive = 100f;             // Vida m�xima
    public float liveRecoverySec = 2f;       // Cu�nto se recupera por segundo
    public float delayBeforeRegen = 3f;      // Segundos sin da�o antes de regenerar
    [SerializeField] private string gameOverScene = "GameOver";
    public event Action OnPlayerDeath;
     public event Action OnPlayerDamaged;


    [Header("Post Processing")]
    public Volume postProcessVolume;

    private Rigidbody rb;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

     // ---- Effect smoothing ----
    private float smoothedT;

    public EyeBlinkEffect eyeBlinkEffect; // asigna desde inspector el script de parpadeo


    public void Awake() //asignar una posicion al jugador
    {
        if (Checkpointmanager.Instance.playerPosition != new Vector3())
        {
            rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
            rb.position = Checkpointmanager.Instance.playerPosition;
        }
    }
    

    // -----------------------------
    //     RECIBIR DA�O
    // -----------------------------
    public void TakeDamage(float damage)
    {

        
        if(live >= maxLive)
        {
            // Actualizar efectos visuales seg�n la vida actual
            InvokeRepeating(nameof(UpdateEffect), 0f, 0.1f);
        }
        live -= damage;
        if (live < 40 & live>39)
        {
            OnPlayerDamaged?.Invoke();
        }
        if (live <= 0)
        {
            live = 0;
            Die();
            return;
        }

        Debug.Log("Vida actual del jugador: " + live);

        // Cancelar regeneraci�n mientras recibe da�o
        CancelInvoke(nameof(RegenerateLife));
        InvokeRepeating(nameof(RegenerateLife), delayBeforeRegen, 0.1f);

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGet(out vignette);
            postProcessVolume.profile.TryGet(out lensDistortion);
            postProcessVolume.profile.TryGet(out chromaticAberration);
            postProcessVolume.weight = 1f;
        }

        
    }

    // -----------------------------
    //     REGENERACI�N DE VIDA
    // -----------------------------
    private void RegenerateLife()
    {
        live += liveRecoverySec;

        if (live >= maxLive)
        {
            live = maxLive;
            CancelInvoke(nameof(RegenerateLife)); // Deja de regenerar al llegar al m�ximo
            CancelInvoke(nameof(UpdateEffect));
            UpdateEffect(); // Actualiza efectos para vida completa
        }

        Debug.Log("Regenerando vida: " + live);
    }

    // -----------------------------
    //     EFECTO DE DESMAYO / T�NEL
    // -----------------------------
    private void UpdateEffect()
    {
        Debug.Log("UpdateEffect");
        float targetT = 1f - (live / maxLive); // 0 = bien, 1 = cr�tico
        
        // Aumentar el efecto inmediatamente si recibimos daño
        if (targetT > smoothedT)
        {
            smoothedT = targetT; // cambio instantáneo al recibir daño
        }
        else
        {
            // Suavizado solo al recuperarse
            smoothedT = Mathf.Lerp(smoothedT, targetT, 0.2f);
        }

        // Pulso visual leve cuando la vida es baja
        float pulse = 0f;
        if (live <= maxLive * 0.3f)
        {
            pulse = Mathf.Sin(Time.time * 3.5f) * 0.1f;
        }

        float finalT = Mathf.Clamp01(smoothedT + pulse);


        // VIGNETTE (visi�n de t�nel)
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(0f, 0.55f, finalT);
            vignette.smoothness.value = Mathf.Lerp(0.25f, 0.85f, finalT);
            vignette.color.value = new Color(0.55f, 0f, 0f);

        }

        // LENS DISTORTION (mareo / deformaci�n de bordes)
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = Mathf.Lerp(0f, -0.45f, finalT);
            lensDistortion.scale.value = Mathf.Lerp(1f, 0.8f, finalT);
        }

        // CHROMATIC ABERRATION (bordes de colores -> desmayo)
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 0.75f, finalT);
        }
    }

    public void Die()
    {

         OnPlayerDeath?.Invoke();
        if (Checkpointmanager.Instance.playerPosition != new Vector3()) //checkpoint en vigor
        {
            Debug.Log("checkpoint existente");
            //Debug.LogWarning(Checkpointmanager.Instance.playerPosition);
            PlayerProgress.Instance.inspectedObjects = Checkpointmanager.Instance.savedIDs;
        } else 
            PlayerProgress.Instance.inspectedObjects.Clear();
        
        if (Cursor.lockState == CursorLockMode.Locked) //si no esta bloqueado lo bloquea
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
       
        // if (eyeBlinkEffect != null)
        // {
        //     // Ejecutar parpadeo antes de cargar la escena
        //     eyeBlinkEffect.Blink(() => SceneManager.LoadScene(gameOverScene));
        // }
        // else
        // {
        //     SceneManager.LoadScene(gameOverScene);
        // }
       }


}
