using System;
using UnityEngine;

public class dieauto : MonoBehaviour
{
   

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        PlayerProgress.Instance.gameObject.GetComponent<ScriptPlayerLive>().Die();
        Debug.Log("dieauto entered");
    }
}
