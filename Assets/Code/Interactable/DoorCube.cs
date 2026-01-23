using UnityEngine;

public class DoorCube : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] requiredInspections;

    private bool isOpen = false;

    public string GetInteractionText()
    {
        if (!CanOpenDoor())
            return "try to open";

        return isOpen ? "close" : "open";
    }

    public void Interact()
    {
        if (!CanOpenDoor())
        {
            Debug.Log("The door is locked... you feel you should inspect something first.");
            return;
        }

        isOpen = !isOpen;
        Debug.Log(isOpen ? "Door opened" : "Door closed");
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
                return false;
        }
        return true;
    }
}