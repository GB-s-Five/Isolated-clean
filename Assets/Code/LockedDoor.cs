using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public AudioClip lockedSound;
    public string message = "Door is locked";
    public float duracionTexto = 2f;   // ← Puedes cambiarlo aquí (1.5f, 2f, etc.)

    private AudioSource audioSource;

    void Awake()
    {
        // Crea el AudioSource automáticamente (así nunca falla)
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    // SONIDO (ahora SÍ suena siempre)
                    if (lockedSound != null)
                    {
                        audioSource.volume = 1f;
                        audioSource.PlayOneShot(lockedSound);
                    }

                    // TEXTO con duración controlada
                    TextoEnPantalla.Mostrar(message, duracionTexto);
                }
            }
        }
    }
}