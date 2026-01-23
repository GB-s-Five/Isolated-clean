using UnityEngine;
using TMPro;
using System.Collections;

public class TextoEnPantalla : MonoBehaviour
{
    public static TextoEnPantalla instance;
    public TextMeshProUGUI textUI;

    void Awake()
    {
        instance = this;
    }

    public static void Mostrar(string msg, float duracion = 2f)
    {
        if (instance == null || instance.textUI == null) return;

        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.MostrarTexto(msg, duracion));
    }

    IEnumerator MostrarTexto(string msg, float duracion)
    {
        textUI.text = msg;
        textUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(duracion);
        textUI.gameObject.SetActive(false);
    }
}