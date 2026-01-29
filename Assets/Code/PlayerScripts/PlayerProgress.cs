using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;

   [SerializeField] public HashSet<string> inspectedObjects = new HashSet<string>();

    // Singleton basico
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // RegisterInspection(string id):
    // lee el ID del objeto si contiene y lo guarda en el Hash
    public void RegisterInspection(string id)
    {
        if (!inspectedObjects.Contains(id))
        {
            inspectedObjects.Add(id);
            Debug.Log($"Registered inspection: {id}");
        }
    }

    // HasInspected(string id):
    // Comprueba si existe este ID ya
    public bool HasInspected(string id)
    {
        return inspectedObjects.Contains(id);
    }
}