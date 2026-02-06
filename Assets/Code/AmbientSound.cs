using UnityEngine;

public class AmbientSound : TriggerSonido
{
    public void Start()
    {
        if (PlayerProgress.Instance.HasInspected(endID))
        {
            PlaySound();
        }
    }
}
