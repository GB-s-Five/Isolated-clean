using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public interface IInteractable
{
    // texto del HUD
    string GetInteractionText();

    // Se llama al interactuar con el objeto
    void Interact();
    bool IsInteractable();
}