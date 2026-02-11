using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class Checkpointmanager : MonoBehaviour

{
    public static Checkpointmanager Instance;

    public HashSet<string> savedIDs = new HashSet<string>();
    public Vector3 playerPosition;
    public bool hasCheckpoint = false;

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
    
    public void SaveInstance(HashSet<string> iDs, Vector3 position)
    {
        savedIDs.Clear();

        foreach (string id in iDs)
            savedIDs.Add(id);

        playerPosition = position;
        hasCheckpoint = true;

        Debug.Log("Checkpointmanager: Saved " + savedIDs.Count + " IDs");
        Debug.Log(String.Join("/", savedIDs));
    }

}
