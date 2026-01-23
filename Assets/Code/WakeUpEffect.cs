using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class WakeUpEffect : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;
    private DepthOfField dof;
    private Bloom bloom;
    private ColorAdjustments colorAdj;

    void Start()
    {
        // Extrae los efectos del Volume Profile
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out colorAdj);

        StartCoroutine(WakeUpSequence());
    }

    private IEnumerator WakeUpSequence()
    {
        float duration = 3f;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            // Vignette
            vignette.intensity.value = Mathf.Lerp(0.55f, 0.1f, p);
            vignette.smoothness.value = Mathf.Lerp(0.65f, 0.3f, p);

            // Depth of Field (URP)
            dof.focusDistance.value = Mathf.Lerp(0.1f, 1.5f, p); // distancia focal del enfoque
            dof.aperture.value = Mathf.Lerp(32f, 5f, p);         // apertura, controla el desenfoque
            dof.focalLength.value = Mathf.Lerp(50f, 35f, p);     // opcional, da efecto de cambio de lente

            // Bloom
            bloom.intensity.value = Mathf.Lerp(1.7f, 1.0f, p);

            // Color adjustments
            colorAdj.postExposure.value = Mathf.Lerp(-0.5f, 0, p);
            colorAdj.contrast.value = Mathf.Lerp(-20f, 0, p);
            colorAdj.saturation.value = Mathf.Lerp(-15f, 0, p);

            yield return null;
        }
    }
}
