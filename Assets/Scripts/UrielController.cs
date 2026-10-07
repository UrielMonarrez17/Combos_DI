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
        
        // Freeze rotation so physics forces don't tip the character over
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Check for ground using a Raycast down from the transform's position
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);

        // Capture input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Direction vector aligned with transform's current orientation
        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        // Control drag to prevent endless sliding on ground
        rb.linearDamping = isGrounded ? dragOnGround : 0f;

        // Jump impulse force
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Reset vertical velocity first for consistent jump height
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Apply movement forces in the physics loop
        if (isGrounded)
        {
            rb.AddForce(moveDirection * moveSpeed, ForceMode.Acceleration);
        }
        else
        {
            // Reduced control force while airborne
            rb.AddForce(moveDirection * moveSpeed * 0.3f, ForceMode.Acceleration);
        }
    }
}