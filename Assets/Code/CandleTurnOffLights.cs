using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class CandleTurnOffLights : MonoBehaviour
{
    //recibe evento del triggerFinDelJuego
    //jugador se congela y no se puede mover
    //se cierra la puerta lentamente 1s y espera otro segundo, tras esto inicia el apagado de luces....

    //el objeto pone en off el component light y y al apagarse emite el sonido, si no tienen siguen la funcion normalmente (public void LuzApagandose) 
    //Los point lights con el codigo se apagan primero, sonido de asignado a uno de los cone-lights "el resto tiene point light"
    //espera 1 segundo
    //los PointLights (30-35 en total) se ponen en una lista y se apagan random en 3 segundos
    //

    [SerializeField] public AudioSource soundController;//controller del sonido
    [SerializeField] public AudioClip soundOffLight;//sonido de cortocircuito, suena al apagarse, SONIDO APAGADO
    [SerializeField] public AudioClip soundOffCandles;  //sonido de vela apagandose

    [Header("All Lights")]
    [SerializeField] private List<Light> spotLights;
    [SerializeField] private List<Light> pointLights;

    //CONTROLES DE PRUEBA
    private bool yaEjecutado = false;



    void Update()
    {
        // Tecla 0 (fila superior del teclado)
        if (Input.GetKeyDown(KeyCode.Alpha0) && !yaEjecutado)
        {
            yaEjecutado = true;
            StartCoroutine(GeneralLightsOff());
        }
    }

    public void LightsOffFinalEventStart()  //recibe de... box collider, bloquea controles jugador, cierra la puerta, manda... LightsOffFinalEventStart()
    {
        StartCoroutine(GeneralLightsOff());

        //faltaran prevenciones
    }



    private IEnumerator GeneralLightsOff()
    {
        yield return new WaitForSeconds(1f);    //se cierra la puerta, espera 1s...
        SpotLightsOff();

        yield return new WaitForSeconds(3f);    //se apagan los spotLights y siguen los pointLights
        yield return StartCoroutine(PointLightsRandomizeOff());

    }


    //se apagan las luces
    private void SpotLightsOff()        //SPOTLIGHTS
    {
        bool reproduceSound = false;

        foreach (Light light in spotLights)
        {
            if (light == null) continue;

            light.enabled = false;

            if (!reproduceSound && soundController && soundOffLight)
            {
                soundController.PlayOneShot(soundOffLight);
                reproduceSound = true;
            }
        }
    }

    private IEnumerator PointLightsRandomizeOff()       //POINTS LIGHTS
    {
        List<Light> lights = new List<Light>(pointLights);

        for (int i = 0; i < lights.Count; i++)
        {
            Light temp = lights[i];
            int randomIndex = Random.Range(i, lights.Count);
            lights[i] = lights[randomIndex];
            lights[randomIndex] = temp;
        }

        float delay = 10f / lights.Count;

        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = false;

                bool reproduceSound = false;    //sonido apagar velas-------------------

                if (!reproduceSound && soundController && soundOffLight)    //----------
                {
                    soundController.PlayOneShot(soundOffCandles);   //------------------
                    reproduceSound = true;                          //------------------
                }           //sonido apagar velas fin-----------------------------------
            }
            yield return new WaitForSeconds(delay);
        }
    }
}