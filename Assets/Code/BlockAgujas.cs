using UnityEngine;

public class BlockAgujas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Con un invkerrepeat llamamos a la funcion para que gire en el rotation x
        InvokeRepeating(nameof(BlockRotationX), 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BlockRotationX()
    {
        transform.Rotate(0f, 5f, 0f);
    }



}
