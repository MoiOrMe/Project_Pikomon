using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    public Transform cible;

    [Header("Vitesse")]
    public float walkSpeed = 3.5f;
    public float sprintSpeed = 6f;

    [Header("Seuil de sprint")]
    public float sprintDistanceThreshold = 10f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Pas de NavMeshAgent trouvé !");
            return;
        }
    }

    void Update()
    {
        if (cible == null || agent == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, cible.position);

        // Sprint si trop loin
        if (distanceToTarget > sprintDistanceThreshold)
        {
            agent.speed = sprintSpeed;
        }
        else
        {
            agent.speed = walkSpeed;
        }

        agent.SetDestination(cible.position);
    }
}
