using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class Padre : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform destino;
     

    void Awake()
    {
        destino = GameObject.FindWithTag("Cosoto").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.autoBraking = true;
    }
    public void Start()
    {
        anim.SetFloat("Speed", 0.8f);

        agent.SetDestination(destino.position);
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Destroy(gameObject);
        }

    }


}
    