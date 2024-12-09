using System.Collections;
using UnityEngine;

public class Plmovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float sprintMultiplier = 3.5f;
    public float sprintDuration = 1f;
    public float sprintCooldown = 1f;
    public float jumpForce = 7f;
    public float groundCheckDistance = 1.5f;
    public float groundCheckRadius = 0.5f; // Radius for SphereCast
    public string groundTag = "Ground";

    private Rigidbody rb;
    private bool canSprint = true;
    private float sprintTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleSprintTimers();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 moveDirection = transform.forward * horizontal;

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && horizontal != 0)
        {
            speed *= sprintMultiplier;
            sprintTimer += Time.deltaTime;
            if (sprintTimer >= sprintDuration)
            {
                canSprint = false;
                cooldownTimer = 0f;
            }
        }

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    void HandleJumping()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out hit, groundCheckDistance))
            {
                // Jump along the slope's normal direction
                Vector3 jumpDirection = hit.normal + Vector3.up;
                rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void HandleSprintTimers()
    {
        if (!canSprint)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= sprintCooldown)
            {
                canSprint = true;
                sprintTimer = 0f;
            }
        }
    }

    bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out RaycastHit hit, groundCheckDistance) &&
               hit.collider.CompareTag(groundTag);
    }
}
