using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UrielController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 15f;
    public float jumpForce = 6f;
    public float dragOnGround = 5f;

    [Header("Ground Check")]
    public float groundDistance = 1.1f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        rb.linearDamping = isGrounded ? dragOnGround : 0f;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection * moveSpeed, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(moveDirection * moveSpeed * 0.3f, ForceMode.Acceleration);
        }
    }
}