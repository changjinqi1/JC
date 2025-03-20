using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Vector2 moveInput;
    private float minX, maxX, minY, maxY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        // Ensure no external physics affect movement
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        SetScreenBounds();
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
        }

    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
        ClampPosition(); // Ensure player stays within screen bounds
    }

 

    void SetScreenBounds()
    {
        Camera mainCamera = Camera.main;
        float screenHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float screenHalfHeight = mainCamera.orthographicSize;

        // Consider collider size to prevent clipping at edges
        float bufferX = playerCollider.bounds.extents.x;
        float bufferY = playerCollider.bounds.extents.y;

        minX = -screenHalfWidth + bufferX;
        maxX = screenHalfWidth - bufferX;
        minY = -screenHalfHeight + bufferY;
        maxY = screenHalfHeight - bufferY;
    }

    void ClampPosition()
    {
        // Restrict player within screen boundaries
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }

}
