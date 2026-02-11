using Unity.VisualScripting;
using UnityEngine;

public enum DoorState
{
    Closed,
    Inverse,
    Reverse
}

public enum PlayerState
{
    Out,
    Inverse,
    Reverse
}

public class DoorController : MonoBehaviour, IInteractable
{

    private DoorState doorState = DoorState.Closed;
    public bool isOpen => (doorState != DoorState.Closed);

    private PlayerState playerState = PlayerState.Out;

    [SerializeField] private string[] requiredInspections;
    
    [SerializeField] private AudioClip sonidoAbrir;
    [SerializeField] private AudioClip sonidoCerrar;
    [SerializeField] private float angulo = 90f;
    public float tiempo = 1.2f;

    private AudioSource audio;
    private Quaternion closedRotation;

    private bool isLocked = false;  // NUEVO 


    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        closedRotation = transform.localRotation;
        if (CanOpenDoor()) UnlockDoor();
    }

    public void LockDoor()
    {
        isLocked = true;
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }

    // -----------------------------------------------------
    // ABRIR PUERTA según la posición del jugador
    // -----------------------------------------------------
    public void SetPlayerInside(bool isInverse)
    {
        if (isInverse) playerState = PlayerState.Inverse;
        else playerState = PlayerState.Reverse;
        //Debug.Log("Player inside: " + playerState.ToString());
    }

    public void SetPlayerOutside()
    {
        playerState = PlayerState.Out;
    }
    public string GetInteractionText()
    {
        return isOpen ? "Close" : "Open";
    }

    public void Toggle()
    {
        // sonido
        if (!isOpen && sonidoAbrir) audio.PlayOneShot(sonidoAbrir);
        int lado = 0;
        if (isOpen)
        {
            doorState = DoorState.Closed;
            lado = 0;
        }
        else
        {
            if (playerState == PlayerState.Inverse)
            {
                doorState = DoorState.Inverse;
                lado = 1;
            }
            else if (playerState == PlayerState.Reverse)
            {
                doorState = DoorState.Reverse;
                lado = -1;
            }
        }

        Quaternion destino = closedRotation * Quaternion.Euler(0, 0, angulo * lado);
        //Debug.Log(destino.eulerAngles + playerState.ToString());

        StopAllCoroutines();
        LockDoor();
        StartCoroutine(Girar(destino));
    }

    System.Collections.IEnumerator Girar(Quaternion final)
    {
        Quaternion inicio = transform.localRotation;
        float t = 0;
        while (t < tiempo)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(inicio, final, t / tiempo);
            yield return null;
        }
        transform.localRotation = final;
        UnlockDoor();
        if (!isOpen)
            audio.PlayOneShot(sonidoCerrar);
        
    }


    public void Interact()
    {
        if (isLocked)
        {
            Debug.LogWarning("The door is locked.");
            return;
        }

        if (!CanOpenDoor())
        {
            Debug.LogWarning("The door is locked... you feel you should inspect something first.");
            return;
        }

        Toggle();

        //Debug.Log(isOpen ? "Door opened" : "Door closed");
    }

    public bool IsInteractable()
    {
        return true;
    }

    private bool CanOpenDoor()
    {
        foreach (string id in requiredInspections)
        {
            if (!PlayerProgress.Instance.HasInspected(id))
            {
                Debug.LogWarning(id);
                Debug.LogWarning(PlayerProgress.Instance.GetAllIDs());
                return false;
            }
        }
        return true;
    }
}

