using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    public float rotationSpeed = 10f;

    private SimplePlayerController playerController;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Aucun Animator trouvé sur le GameObject !");
            return;
        }

        playerController = GetComponent<SimplePlayerController>();
        if (playerController == null)
        {
            Debug.LogError("Aucun SimplePlayerController trouvé !");
            return;
        }
    }

    void Update()
    {
        if (animator == null || playerController == null) return;

        Vector3 moveDir = playerController.lastMoveDirection;

        float speed = playerController.currentSpeed;

        animator.SetFloat("Speed", speed);

        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
