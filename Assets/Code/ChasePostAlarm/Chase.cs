using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Perseguidor : MonoBehaviour
{
    [Header("=== PERSECUCIÓN ===")]
    public float velocity = 0.8f;
    public Transform player;

    [Header("=== NAVMESH ===")]
    public NavMeshAgent agent;

    [Header("=== ANIMATOR ===")]
    public string parameterSpeed = "Speed";
    public Animator animator;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject trampaTriggerMurmullos;
    [SerializeField] private GameObject foto;
    [SerializeField] private GameObject[] objetosBorrar;
    
    public AudioSource bum;





    private bool chase = false;

    void Update()
    {
        if (!chase || !player || !agent) return;

        agent.SetDestination(player.position);
        Debug.Log(agent.velocity.magnitude + "Velocidad");
        if (animator)
        {
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat(parameterSpeed, speedPercent);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            CatchPlayer();
        }
    }

    void CatchPlayer()
    {
        chase = false;

        Debug.Log("JUGADOR ALCANZADO");

        agent.isStopped = true;

        audioSource.Pause();
        bum.Play();

        if (animator)
            animator.SetFloat(parameterSpeed, 0f);

        gameObject.SetActive(false);

        if (trampaTriggerMurmullos != null) {
            trampaTriggerMurmullos.SetActive(true);
            trampaTriggerMurmullos.GetComponent<AudioSource>().Play();
        }

        if (foto != null) {
            foto.SetActive(true); 
        }

        
        foreach (GameObject gameObject in objetosBorrar)
        {
            if (gameObject != null)
                gameObject.SetActive(false);
           
        }
        
    }
    // LLAMADO DESDE ZONASEGURA
    public void ActiveChase()
    {
        audioSource.Play();

        if (!agent)
            agent = GetComponent<NavMeshAgent>();

        agent.speed = velocity;

        agent.enabled = true;
        chase = true;
        agent.isStopped = false;
        

        Debug.Log("EL MODELO COMIENZA A PERSEGUIR");
    }
}
