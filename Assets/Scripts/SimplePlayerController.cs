using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    private CharacterController characterController;

    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;
        Debug.Log($"Is Grounded: {isGrounded}");

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
