using UnityEngine;
using UnityEngine.LightTransport;

public class EventoFinalController : MonoBehaviour
{
    [SerializeField] private GameObject murmurosTrampaTrigger;
    [SerializeField] private GameObject fotoTrampa;

    [SerializeField] private string requiredEventID = "PapasustoRealizado";

    private bool papaAsustado = false;


    private void Start()
    {
        fotoTrampa.SetActive(false);
        murmurosTrampaTrigger.SetActive(false);

        if (PlayerProgress.Instance != null && PlayerProgress.Instance.HasInspected(requiredEventID))
        {
            papaAsustado = true;
        }
    }


    private void Update()
    {
        if (PlayerProgress.Instance != null && PlayerProgress.Instance.HasInspected(requiredEventID))
        {
            papaAsustado = true;
            if (papaAsustado)
            {
                ActivarFinal();
                papaAsustado = false;
            }
        }
    }
    private void ActivarFinal()
    {
        fotoTrampa.SetActive(true);
        murmurosTrampaTrigger.SetActive(true);
    }
}
