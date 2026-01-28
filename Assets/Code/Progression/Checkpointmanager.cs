using System.Collections.Generic;
using UnityEngine;

public class Checkpointmanager : MonoBehaviour
{
    public static Checkpointmanager Instance;

    public HashSet<string> savedIDs = new HashSet<string>(); //zero tags
    public Vector3 playerPosition; //starting pos
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.LogWarning(playerPosition);
    }


    public void SaveInstance(HashSet<string> iDs, Vector3 position)
    {
        savedIDs.Clear();
        foreach (string id in iDs)
        {
            savedIDs.Add(id);
        }
        playerPosition = position;
    }
}
