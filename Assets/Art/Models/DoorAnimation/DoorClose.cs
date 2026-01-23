using UnityEngine;

public class DoorClose : MonoBehaviour
{
    [SerializeField] private DoorController controller;

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return; //modificacion
        //controller.CloseDoor();
    }
}
