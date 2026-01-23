using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;

    private Quaternion _closeRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCororutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _closeRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) //error Input sistem
        {
            if (_currentCororutine != null) StopCoroutine(_currentCororutine);
            _currentCororutine = StartCoroutine(ToggleDoor());
        }
    }

    private IEnumerator ToggleDoor()
    {
        Quaternion targetRotation = isOpen ? _closeRotation : _openRotation;
        isOpen = !isOpen;

        while (Quaternion.Angle(transform.rotation, targetRotation) >0.02f) //original ...>0.01f) haber que pasa
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
