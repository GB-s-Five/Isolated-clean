using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ScriptPlayerLive : MonoBehaviour
{
    public float live = 100f;                // Vida actual
    public float maxLive = 100f;             // Vida m�xima
    public float liveRecoverySec = 2f;       // Cu�nto se recupera por segundo
    public float delayBeforeRegen = 3f;      // Segundos sin da�o antes de regenerar

    [Header("Post Processing")]
    public Volume postProcessVolume;

    private Vignette vignette;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;



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
        float t = 1f - (live / maxLive); // 0 = bien, 1 = cr�tico

        // VIGNETTE (visi�n de t�nel)
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(0f, 1f, t);
            vignette.smoothness.value = Mathf.Lerp(0f, 1f, t);
        }

        // LENS DISTORTION (mareo / deformaci�n de bordes)
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = Mathf.Lerp(0f, -0.6f, t);
            lensDistortion.scale.value = Mathf.Lerp(1f, 0.8f, t);
        }

        // CHROMATIC ABERRATION (bordes de colores -> desmayo)
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, t);
        }
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        
        //transform.position = new Vector3(20.6430702f,1.01699996f,18.1334991f);
        //transform.rotation = Quaternion.identity;
    }


}
