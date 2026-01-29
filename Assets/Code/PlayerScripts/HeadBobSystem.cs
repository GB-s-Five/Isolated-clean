using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBobSystem : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private float walkAmount = 0.004f; // Valor de movimiento para andar
    [SerializeField] private float walkFrequency = 10f; // Valor de la frecuencia
    [SerializeField] private float walkStepInterval = 0.55f; // MISMO valor que Footsteps 

    [Header("Run")]
    [SerializeField] private float runAmount = 0.007f; // Valor de movimiento al correr
    [SerializeField] private float runFrequency = 14f; // Valor de la frecuencia al correr
    [SerializeField] private float runStepInterval = 0.35f; // MISMO valor que Footsteps

    [Header("Smooth")]
    [SerializeField] private float smooth = 8f; // Float para darle un valor al suavizado del movimiento

    private Vector3 startPos; // Variable privada de vector3 para guardar la posicion inicial
    private float timer; // Timer para controlar el sin y cos
    private float stepDelayTimer; // Float para esperar al primer paso
    private bool canBob; // Variable booleana para saber si puede cabecear

    private void Start()
    {
        startPos = transform.localPosition; // Se guarda la posicion local del transform como posicion inicial
    }

    private void Update()
    {
        bool isMoving = IsMoving(); 
        bool isRunning = Keyboard.current.leftAltKey.isPressed; // Se considera que esta corriendo si la tecla alt esta presionada.

        if (!isMoving) // En caso de que no se esta moviendo
        {
            ResetHeadBob(); // Se llama a la funcion de ResetHeadBob
            timer = 0f; // Se establece el timer a 0
            stepDelayTimer = 0f; // Se establece el timer para el primer paso en 0
            canBob = false; // Se establece la variable canBob en false (aasi no puede hacer el movimiento por defecto)
            return;
        }

        float currentInterval = isRunning ? runStepInterval : walkStepInterval; // Selecciona el intervalo de los pasos basandose en si esta corriendo o no. Lo pasa al float llamado currentInterval.

        // Espera a que pase el tiempo del primer paso
        if (!canBob)  // entra en el IF si canBob es false
        {
            stepDelayTimer += Time.deltaTime;

            if (stepDelayTimer >= currentInterval) // Si el delay del tiempo es mayor o igual al intervalo actual permite la posibilidad de realizar el cabeceo
            {
                canBob = true; // Pone el cabeceo en true para que pueda saltar
                timer = 0f; // empieza sincronizado
            }
            return;
        }

        DoHeadBob(isRunning); // llama a la funcion de DoHeadBob para hacer el cabeceo
    }

    private void DoHeadBob(bool isRunning)
    {
        float amount = isRunning ? runAmount : walkAmount; // Decide el movimiento dependiendo de si esta corriendo o no
        float frequency = isRunning ? runFrequency : walkFrequency;

        timer += Time.deltaTime * frequency;

        float y = Mathf.Sin(timer) * amount; // calcula el movimiento Y
        float x = Mathf.Cos(timer * 0.5f) * amount * 1.5f; // Calcula el movimiento X

        Vector3 targetPos = startPos + new Vector3(x, y, 0f); // Calcula la posicion del objetivo

        transform.localPosition = Vector3.Lerp( // Lerp (Interpolación Lineal) es una función que calcula un punto entre dos valores (A y B) en función de un tercer parámetro 't' (entre 0 y 1)
            transform.localPosition,
            targetPos, // Se le pasa la ubicacion objetivo para realizar el movimiento del cabeceo.
            smooth * Time.deltaTime // Time.deltaTime // variable esencial que representa el tiempo (en segundos) transcurrido desde el último fotograma
        );
    }

    private void ResetHeadBob() // Se resetea el movimiento del cabeceo 
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            startPos, // Se le pasa la posicion incial para reiniciar o resetear el movimiento
            smooth * Time.deltaTime // Time.deltaTime // variable esencial que representa el tiempo (en segundos) transcurrido desde el último fotograma
        );
    }
    public void ForceReset()
    {
        timer = 0f;
        stepDelayTimer = 0f;
        canBob = false;
        transform.localPosition = startPos;
    }


    private bool IsMoving() // Decide si se esta moviendo en base a la presion de cada tecla
    {
        if (!enabled) return false; // si el script esta desactivado retorna false por lo que nunca llega a leer que las letras estan presionadas.

        return Keyboard.current.wKey.isPressed ||
            Keyboard.current.aKey.isPressed ||
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.dKey.isPressed;
    }

}
