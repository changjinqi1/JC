using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hookmovement : MonoBehaviour
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        horizontalInput = -horizontalInput;

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