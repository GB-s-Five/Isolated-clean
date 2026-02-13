using UnityEngine;

public class GameTool : MonoBehaviour
{
    [Header("Inspectable Object ID")]

    [SerializeField] private bool objectId = false;
    [SerializeField] private bool objectId1 = false;
    [SerializeField] private bool objectId2 = false;
    [SerializeField] private bool objectId3 = false;
    [SerializeField] private bool objectId4 = false;
    [SerializeField] private bool objectId5 = false;

    [SerializeField] private string objectID = "FamiliarPhoto";
    [SerializeField] private string objectID1 = "MedicalReport";
    [SerializeField] private string objectID2 = "LightMessage";
    [SerializeField] private string objectID3 = "Rayos";
    [SerializeField] private string objectID4 = "ViewXray";
    [SerializeField] private string objectID5 = "TelephoneHeared";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (objectId)
        {
            PlayerProgress.Instance.RegisterInspection(objectID);
        }
         if (objectId1)
        {
            Debug.Log("ENtrado");
            PlayerProgress.Instance.RegisterInspection(objectID1);
        }
         if (objectId2) {
            PlayerProgress.Instance.RegisterInspection(objectID2);

        }
         if (objectId3) {
            PlayerProgress.Instance.RegisterInspection(objectID3);

        }
         if (objectId4) {
            PlayerProgress.Instance.RegisterInspection(objectID4);

        }
         if (objectId5) {
            PlayerProgress.Instance.RegisterInspection(objectID5);

        }


    }

}
