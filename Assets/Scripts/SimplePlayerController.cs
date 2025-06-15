using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    private CharacterController characterController;

    public float speed = 5f;
    public float speedRunning = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    public float speedSmoothTime = 0.1f;
    private float speedSmoothVelocity;

    public Transform cameraTransform;

    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    [HideInInspector] public Vector3 lastMoveDirection;
    public float currentSpeed;

    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (canMove)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = cameraTransform.right;

            Vector3 move = (camForward * input.z + camRight * input.x).normalized;
            lastMoveDirection = move;

            float appliedSpeed = Input.GetKey("left shift") ? speedRunning : speed;
            float targetSpeed = move.magnitude * appliedSpeed;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
            characterController.Move(move * appliedSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void EnableMovement()
    {
        canMove = true;
    }

}
