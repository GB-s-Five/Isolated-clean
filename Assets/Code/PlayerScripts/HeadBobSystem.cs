using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBobSystem : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private float walkAmount = 0.004f; // Cuanto se mueve la camara // SerializeField para poder editarlo desde el inspector
    [SerializeField] private float walkFrequency = 10f; // Frecuencia de movimiento (veces)

    [Header("Run")]
    [SerializeField] private float runAmount = 0.007f; // Cuanto se mueve la camara // SerializeField para poder editarlo desde el inspector
    [SerializeField] private float runFrequency = 14f; // Frecuencia de movimiento (veces) (Corriendo)

    [Header("Smooth")]
    [SerializeField] private float smooth = 8f; // Controla como de suave es la transicion del movimiento 

    private Vector3 startPos; // Posicion original o inicial de la camara
    private float timer; // Float (valor numerico) para controlar el ritmo del movimiento (sin y cos)

    private void Start()
    {
        startPos = transform.localPosition; // Guardar la posicion inicial de la camara para dejarla en el mismo sitio cuando termine el headbob
    }

    private void Update()
    {
        bool isMoving = IsMoving(); // Booleano (Si/No) de si esta en movimiento
        bool isRunning = Keyboard.current.leftAltKey.isPressed; // Booleano (Si/No) de comprobar  si aprieta el alt (correr)

        if (isMoving)
        {
            DoHeadBob(isRunning); // Si esta moviendose hacer el movimiento de la camara
        }
        else
        {
            ResetHeadBob(); // De lo contrario vuelve la camara a la posicion inicial
            timer = 0f; // Reinicia el tiempo para que el movimiento empeice en 0 , desde el inicio
        }
    }

    private void DoHeadBob(bool isRunning) // Funcion de DoHeadBob, depende de la variable booleana de isMoving
    {
        float amount = isRunning ? runAmount : walkAmount; // Cambio en amount. En caso de que isRunning sea true coge el valor de runAmount
        float frequency = isRunning ? runFrequency : walkFrequency; // Cambio en frequency. En caso de que isRunning sea true coge el valor de runFrequency.

        timer += Time.deltaTime * frequency; // El movimeinto es conitnuo y depende del framerate.

        float y = Mathf.Sin(timer) * amount; //Movimiento en Y
        float x = Mathf.Cos(timer * 0.5f) * amount * 1.5f; // Movimiento en X

        Vector3 targetPos = startPos + new Vector3(x, y, 0f); // Aplica el movimiento en los vectores.

        // Suaviza el moviento //  Lerp (Interpolación Lineal) es una función que calcula un punto entre dos valores (A y B) en función de un tercer parámetro 't' (entre 0 y 1)
        transform.localPosition = Vector3.Lerp( 
            transform.localPosition,
            targetPos, // Posicion objetivo (Movimiento maximo)
            smooth * Time.deltaTime
        );
    }

    private void ResetHeadBob() // Se resetea el movimiento . se le pasa a la funcion Lerp la posicion inicial para que se reinicie por completo
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            startPos, // Posicion Inicial
            smooth * Time.deltaTime
        );
    }

    private bool IsMoving() // Se decide cuando esta moviendose si alguna de las teclas marcadas abajo esta presionada.
    {
        return Keyboard.current.wKey.isPressed || // las dos barritas es como un break ||
               Keyboard.current.aKey.isPressed ||
               Keyboard.current.sKey.isPressed ||
               Keyboard.current.dKey.isPressed;
    }
}
