using UnityEngine;

public class DoorTrigger : MonoBehaviour 
{
    [SerializeField] private DoorController controller;
    [SerializeField] private bool isInverse;
    //private bool isOpen = false;

   private void OnTriggerEnter(Collider other)
   {
        if (other.gameObject.tag == "Player")
        {
            controller.SetPlayerInside(isInverse);
        }
        
   }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            controller.SetPlayerOutside();
        }
    }
}
