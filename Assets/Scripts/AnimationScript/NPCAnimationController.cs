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
            Debug.LogError("Aucun Animator trouv� sur le PNJ !");
        if (agent == null)
            Debug.LogError("Aucun NavMeshAgent trouv� sur le PNJ !");
    }

    void Update()
    {
        if (animator == null || agent == null) return;

        // 1. Vitesse r�elle (en m/s)
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        // 2. Orientation du PNJ vers sa direction de d�placement
        if (agent.velocity.sqrMagnitude > 0.01f) // Pour �viter de tourner dans le vide
        {
            Quaternion toRotation = Quaternion.LookRotation(agent.velocity.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
