using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FootstepsController : MonoBehaviour
{
    [Header("Clips de pasos")]
    [SerializeField] private AudioClip[] footstepClips;

    [Header("Referencia al AudioSource (se creará si no existe)")]
    [SerializeField] private AudioSource audioSource;

    [Header("Frecuencia de pasos")]
    [SerializeField] private float walkStepInterval = 0.55f;
    [SerializeField] private float runStepInterval = 0.35f;

    private float stepTimer = 0f;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.4f; // volumen más bajo
        }
    }

    private void Update()
    {
        bool isMoving = IsMoving();
        bool isLeaning = Keyboard.current.qKey.isPressed || Keyboard.current.eKey.isPressed;
        bool isRunning = Keyboard.current.leftAltKey.isPressed;

        if (!isMoving && !isLeaning)
        {
            stepTimer = 0f;
            return;
        }

        float currentInterval = isRunning ? runStepInterval : walkStepInterval;
        stepTimer += Time.deltaTime;

        if (stepTimer >= currentInterval)
        {
            PlayRandomFootstep();
            stepTimer = 0f;
        }
    }

    private bool IsMoving()
    {
        return Keyboard.current.wKey.isPressed ||
               Keyboard.current.aKey.isPressed ||
               Keyboard.current.sKey.isPressed ||
               Keyboard.current.dKey.isPressed;
    }

    private void PlayRandomFootstep()
    {
        if (footstepClips.Length == 0) return;

        int rand = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[rand]);
    }
}
