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

    private float live;                // Vida actual
    public float maxLive = 100f;             // Vida m�xima
    public float liveRecoverySec = 2f;       // Cu�nto se recupera por segundo
    public float delayBeforeRegen = 3f;      // Segundos sin da�o antes de regenerar
    [SerializeField] private string gameOverScene = "GameOver";
    public event Action OnPlayerDeath;
    public event Action OnPlayerDamaged;
    private bool hasblinked = false;
    [SerializeField] private AudioSource heartbeatSource;
    [SerializeField] private float minVolume = 0.05f;
    [SerializeField] private float maxVolume = 1.5f;
    [SerializeField] private float minPitch = 1f;
    [SerializeField] private float maxPitch = 5f;
    [SerializeField] private float volumeStartLife = 99f;
    [SerializeField] private float volumeFullLife = 10f;
    [SerializeField] private float pitchMaxLife = 1f;



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
        live = maxLive;
        if (Checkpointmanager.Instance.playerPosition != new Vector3())
        {
            rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
            rb.position = Checkpointmanager.Instance.playerPosition;
        }
    }
    

    // -----------------------------
    //     RECIBIR DAÑO
    // -----------------------------
    public void TakeDamage(float damage)
    {

        
        if(live >= maxLive)
        {
            // Actualizar efectos visuales seg�n la vida actual
            InvokeRepeating(nameof(UpdateEffect), 0f, 0.1f);
        }
        live -= damage;
        if (live < 40 && !hasblinked)
        {
            OnPlayerDamaged?.Invoke();
            hasblinked = !hasblinked;
        }
        if (live < 0)
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
        UpdateHeartbeat();
    }

    private void UpdateHeartbeat()
    {
        if (live >= maxLive)
        {
            if (heartbeatSource.isPlaying)
                heartbeatSource.Stop();
            return;
        }
        if (!heartbeatSource.isPlaying)
            heartbeatSource.Play();
        // volume
        float volumeT = Mathf.InverseLerp(volumeStartLife, volumeFullLife, live);
        float volume = Mathf.Lerp(minVolume, maxVolume, volumeT);
        // velocity
        float pitchT = Mathf.InverseLerp(maxLive, pitchMaxLife, live);
        float pitch = Mathf.Lerp(minPitch, maxPitch, pitchT);
        heartbeatSource.volume = volume;
        heartbeatSource.pitch = pitch;
    }

    
    // -----------------------------
    //     REGENERACIóN DE VIDA
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
        if (live > 40 && hasblinked)
        {
            hasblinked = !hasblinked;
        }
        UpdateHeartbeat();
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
        Debug.LogWarning(PlayerProgress.Instance.GetAllIDs());
        
        if (Checkpointmanager.Instance.playerPosition != new Vector3()) //checkpoint en vigor
        {
            Debug.LogWarning("checkpoint existente");
            //Debug.LogWarning(Checkpointmanager.Instance.playerPosition);
            PlayerProgress.Instance.inspectedObjects.Clear();

            foreach (string id in Checkpointmanager.Instance.savedIDs)
            {
                PlayerProgress.Instance.inspectedObjects.Add(id);
            }

        } else 
            PlayerProgress.Instance.inspectedObjects.Clear();
        
        if (Cursor.lockState == CursorLockMode.Locked) //si no esta bloqueado lo bloquea
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        OnPlayerDeath?.Invoke();
       }


}
