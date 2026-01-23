using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private Transform playerBody;


    private PlayerControls controls;
    private Vector2 lookInput;
    private float xRotation = 0f;

    // Awake():
    // Se ejecuta al iniciar el objeto. Inicializa los controles del jugador y
    // asigna las acciones de entrada para el movimiento de la cámara (mirar con el ratón o joystick).
    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    // OnEnable():
    // Activa los controles del jugador cuando el objeto se habilita en la escena.
    private void OnEnable() => controls.Player.Enable();

    // OnDisable():
    // Desactiva los controles del jugador cuando el objeto se deshabilita.
    private void OnDisable() => controls.Player.Disable();

    // Update():
    // Se ejecuta cada frame. Controla la rotación de la cámara (mirar arriba y abajo)
    // y la rotación del cuerpo del jugador (mirar a los lados) según la entrada del ratón.
    private void Update()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (mouseX != 0)
        {
            playerBody.Rotate(Vector3.up * mouseX);
            Debug.Log("Rotate");
        }
        //Debug.Log(mouseX);
    }
}