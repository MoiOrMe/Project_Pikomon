using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPCAnimationController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            Debug.LogError("Aucun Animator trouvé sur le PNJ !");
        if (agent == null)
            Debug.LogError("Aucun NavMeshAgent trouvé sur le PNJ !");
    }

    void Update()
    {
        if (animator == null || agent == null) return;

        // 1. Vitesse réelle (en m/s)
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        // 2. Orientation du PNJ vers sa direction de déplacement
        if (agent.velocity.sqrMagnitude > 0.01f) // Pour éviter de tourner dans le vide
        {
            Quaternion toRotation = Quaternion.LookRotation(agent.velocity.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
