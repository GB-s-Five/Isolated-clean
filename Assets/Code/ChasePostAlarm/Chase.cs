using UnityEngine;

public class Perseguidor : MonoBehaviour
{
    [Header("=== PERSECUCIÓN ===")]
    public float velocity = 4f;
    public Transform player;
    public float distanceToCatch = 1.2f;

    [Header("=== ANIMATOR ===")]
    public string parameterSpeed = "Speed";
    public float speedSprint = 0.8f; // > 0.7
    
    public Animator animator;

    private bool chase = false;

    private void Update()
    {
        if (!chase || player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * velocity * Time.deltaTime;

        // que mire al jugador
        transform.LookAt(player);

         // COMPROBAR SI ALCANZA AL JUGADOR
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= distanceToCatch)
        {
            CatchPlayer();
        }
    }
    void CatchPlayer()
    {
        chase = false;

        Debug.Log("JUGADOR ALCANZADO");

        // Parar animación
        if (animator != null)
            animator.SetFloat(parameterSpeed, 0f);

        // DESAPARECER PADRE
        if (transform.parent != null)
            transform.parent.gameObject.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    // LLAMADO DESDE ZONASEGURA
    public void ActiveChase()
    {
        chase = true;
        if (animator != null)
            animator.SetFloat(parameterSpeed, speedSprint);
        Debug.Log("EL MODELO COMIENZA A PERSEGUIR");
    }
}
