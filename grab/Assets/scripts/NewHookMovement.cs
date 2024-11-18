using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHookMovement : MonoBehaviour
{
    public float speed = 9f;
    public Vector3 minBounds = new Vector3(0, 5, -8);
    public Vector3 maxBounds = new Vector3(0, 18, 8);
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    void FixedUpdate()
    {
        // Horizontal movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        horizontalInput = -horizontalInput; // Invert horizontal input

        // Vertical movement handled exclusively by right mouse button
        float verticalInput = 0;

        if (Input.GetMouseButton(1)) // Right mouse button pressed
        {
            verticalInput = -1; // Move down
        }
        else if (transform.position.y < maxBounds.y) // Right mouse button released
        {
            verticalInput = 1; // Move up
        }

        // Apply movement
        if (horizontalInput != 0 || verticalInput != 0)
        {
            rb.isKinematic = false;
            Vector3 move = new Vector3(0, verticalInput, horizontalInput) * speed;
            rb.velocity = move;
        }
        else
        {
            StopAndFixMovement();
        }

        // Clamp position within bounds
        Vector3 clampedPosition = new Vector3(
            rb.position.x,
            Mathf.Clamp(rb.position.y, minBounds.y, maxBounds.y),
            Mathf.Clamp(rb.position.z, minBounds.z, maxBounds.z)
        );

        rb.position = clampedPosition;
    }

    void StopAndFixMovement()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
