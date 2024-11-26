using System.Collections;
using UnityEngine;

public class Plmovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float sprintMultiplier = 3.5f;
    public float sprintDuration = 1f;
    public float sprintCooldown = 1f;
    public float jumpForce = 7;
    public float groundCheckDistance = 1.5f;
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
        float horizontal = Input.GetAxis("Vertical");
        float vertical = Input.GetAxis("Horizontal");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
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
        bool isGrounded = IsGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
