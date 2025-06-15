using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private int isWalkingHash;
    private int isRunningHash;

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
            Debug.LogError("Aucun SimplePlayerController trouvé sur le GameObject !");
            return;
        }

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        if (animator == null || playerController == null) return;

        Vector3 moveDir = playerController.lastMoveDirection;

        bool isWalking = moveDir.magnitude > 0f;
        bool isRunning = isWalking && Input.GetKey(KeyCode.LeftShift);

        animator.SetBool(isWalkingHash, isWalking);
        animator.SetBool(isRunningHash, isRunning);

        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
