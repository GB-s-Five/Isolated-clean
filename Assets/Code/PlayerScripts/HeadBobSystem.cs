using UnityEngine;

[RequireComponent(typeof(Transform))]
public class HeadBobSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody playerRb;

    [Header("Speed Thresholds")]
    [SerializeField] private float minSpeedToBob = 0.2f;   // debajo de esto NO hay head bob
    [SerializeField] private float walkSpeed = 3f;        // velocidad típica andando
    [SerializeField] private float runSpeed = 7f;         // velocidad típica corriendo

    [Header("Bob Amount")]
    [SerializeField] private float walkAmount = 0.004f;
    [SerializeField] private float runAmount = 0.008f;

    [Header("Bob Frequency")]
    [SerializeField] private float walkFrequency = 10f;
    [SerializeField] private float runFrequency = 16f;

    [Header("Smooth")]
    [SerializeField] private float smooth = 8f;

    private Vector3 startPos;
    private float timer;

    private void Start()
    {
        startPos = transform.localPosition;

        if (!playerRb)
            Debug.LogError("HeadBobSystem: Rigidbody no asignado");
    }

    private void Update()
    {
        if (!playerRb)
            return;

        // velocidad REAL del cuerpo (plano horizontal)
        Vector3 horizontalVel = playerRb.linearVelocity;
        horizontalVel.y = 0f;

        float speed = horizontalVel.magnitude;

        // si no se mueve lo suficiente → reset
        if (speed < minSpeedToBob)
        {
            ResetHeadBob();
            timer = 0f;
            return;
        }

        DoHeadBob(speed);
    }

    private void DoHeadBob(float speed)
    {
        // normalizamos la velocidad (walk → run)
        float t = Mathf.InverseLerp(walkSpeed, runSpeed, speed);

        float amount = Mathf.Lerp(walkAmount, runAmount, t);
        float frequency = Mathf.Lerp(walkFrequency, runFrequency, t);

        timer += Time.deltaTime * frequency;

        float y = Mathf.Sin(timer) * amount;
        float x = Mathf.Cos(timer * 0.5f) * amount * 1.5f;

        Vector3 targetPos = startPos + new Vector3(x, y, 0f);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            smooth * Time.deltaTime
        );
    }

    private void ResetHeadBob()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            startPos,
            smooth * Time.deltaTime
        );
    }

    public void ForceReset()
    {
        timer = 0f;
        transform.localPosition = startPos;
    }
}
