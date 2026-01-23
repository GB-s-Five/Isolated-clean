using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    
    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float cameraHeight = 1.8f;
    
    [Header("Lean Body")]
    [SerializeField] private float maxBodyLeanOffset = 0.25f;
    [SerializeField] private float leanSmooth = 10f;

    private PlayerControls controls;
    private Rigidbody rb;
    private Vector2 moveInput;
    private float leanInput = 0f;
    private bool isSprinting = false;

    private float currentBodyLean = 0f;
    private float previousBodyLean = 0f;

    public float LeanInput => leanInput;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Lean.performed += ctx => leanInput = ctx.ReadValue<float>();
        controls.Player.Lean.canceled += ctx => leanInput = 0f;

        controls.Player.Sprint.started += ctx => isSprinting = true;
        controls.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    private void OnEnable() => controls.Player.Enable();

    // OnDisable():
    // Desactiva los controles del jugador al deshabilitar el objeto.
    private void OnDisable() => controls.Player.Disable();



    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Cursor.lockState != CursorLockMode.Locked) //si no esta bloqueado lo bloquea
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        if (playerCamera != null)
        {
            Vector3 camPos = playerCamera.localPosition;
            camPos.y = cameraHeight;
            playerCamera.localPosition = camPos;
        }
    }

    // Update():
    // Se ejecuta cada frame. 
    private void Update()
    {
        
    }

    // FixedUpdate():
    // Se ejecuta en intervalos fijos. Calcula y aplica el movimiento del jugador
    // usando física (Rigidbody), según la entrada de movimiento.

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(moveInput.x, 0, moveInput.y);
        input = transform.TransformDirection(input);

        float speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector3 vel = input * speed;

        float targetBodyLean = leanInput * maxBodyLeanOffset;
        currentBodyLean = Mathf.Lerp(currentBodyLean, targetBodyLean, Time.fixedDeltaTime * leanSmooth);
        Vector3 leanDelta = transform.right * (currentBodyLean - previousBodyLean);
        previousBodyLean = currentBodyLean;

        Vector3 final = vel + leanDelta / Time.fixedDeltaTime;

        rb.linearVelocity = new Vector3(final.x, rb.linearVelocity.y, final.z);
    }
}