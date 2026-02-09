using System.Collections;
using UnityEngine;

public class FlameCandleLight : MonoBehaviour
{

    [SerializeField] public float lightBaseIntensity;   
    [SerializeField] public float lightScale = 0.005f;
    [SerializeField] public float bucleTime = 0.07f;

    private Light pointLight;

    void Awake()
    {
        pointLight = GetComponent<Light>();
    }

    void Start()
    {
        lightBaseIntensity = pointLight.intensity;
        StartCoroutine(lightLoop());

    }

    IEnumerator lightLoop()
    {
        while (true)
        {
            pointLight.intensity = lightBaseIntensity + Random.Range(-lightScale, lightScale);


            yield return new WaitForSeconds(bucleTime);
        }
    }
}
