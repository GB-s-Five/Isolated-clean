using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class FlickeringLight : MonoBehaviour
{
    public Light flickerLight;
    public Light secondaryLight;

    [Header("Timing")]
    public float minOnTime = 0.05f;
    public float maxOnTime = 0.3f;
    public float minOffTime = 0.05f;
    public float maxOffTime = 0.5f;

    [Header("Intensity Flicker")]
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.5f;

    [Header("Chance of Big Glitch")]
    public float glitchChance = 0.15f;
    private void OnEnable()
    {
        if (!flickerLight)
            flickerLight = GetComponent<Light>();

        StartCoroutine(FlickerRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        flickerLight.enabled = false;
        secondaryLight.enabled = false;
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Chance to do a creepy glitch burst
            if (Random.value < glitchChance)
            {
                int glitchCount = Random.Range(3, 8);

                for (int i = 0; i < glitchCount; i++)
                {
                    flickerLight.enabled = !flickerLight.enabled;
                    if (secondaryLight)
                        secondaryLight.enabled = flickerLight.enabled;
                    flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
                    yield return new WaitForSeconds(Random.Range(0.02f, 0.08f));
                }
            }

            // Normal flicker behavior
            flickerLight.enabled = true;
            if (secondaryLight)
                secondaryLight.enabled = true;
            flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));

            flickerLight.enabled = false;
            if (secondaryLight)
                secondaryLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));
        }
    }
}
