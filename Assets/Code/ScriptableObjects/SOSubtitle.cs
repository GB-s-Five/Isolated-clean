using UnityEngine;

[CreateAssetMenu(fileName = "SOSubtitle", menuName = "Scriptable Objects/SOSubtitle")]
public class SOSubtitle : ScriptableObject
{
    public AudioClip audioClip;
    public SAudioSubtitle[] messageFragments;
    
}
