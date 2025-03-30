using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform paddle; // Assign in Inspector
    private bool isLaunched = false;
    private Vector2 direction;
    private Vector2 launchOffset = new Vector2(0f, 0.5f);

    private float launchTime = -1f;
    private float ignorePaddleDuration = 0.1f;

    private int bounceLayerMask; // Computed in Start()

    void Start()
    {
        direction = Vector2.zero;

        // ✅ Safely set the layer mask here to ignore the Ball layer
        bounceLayerMask = ~LayerMask.GetMask("Ball"); // Exclude Ball layer (e.g. index 3)
    }

    void FixedUpdate()
    {
        if (!isLaunched)
        {
            // Stick to paddle before launch
            transform.position = paddle.position + (Vector3)launchOffset;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Launch at 75-degree angle
                float angle = 75f * Mathf.Deg2Rad;
                direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
                isLaunched = true;
                launchTime = Time.time;

                // Nudge slightly off the paddle
                transform.position += (Vector3)(direction * 0.05f);

                Debug.Log("Ball launched with direction: " + direction);
            }
        }
        else
        {
            float moveDistance = speed * Time.fixedDeltaTime;
            Vector2 move = direction * moveDistance;

            // 🔧 Raycast excluding "Ball" layer
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, moveDistance, bounceLayerMask);

            Debug.DrawRay(transform.position, direction * moveDistance, Color.cyan, 0.1f);
            Debug.Log($"Ball Pos: {transform.position} | Dir: {direction} | Move: {move}");

            if (hit.collider != null)
            {
                Debug.Log($"Hit: {hit.collider.name}");

                string tag = hit.collider.tag;

                // Ignore paddle if still within launch immunity
                if (tag == "Paddle" && (Time.time - launchTime) < ignorePaddleDuration)
                {
                    Debug.Log("Ignoring paddle hit due to launch delay.");
                    transform.position += (Vector3)move;
                    return;
                }

                // Bounce logic
                if (tag == "Paddle")
                {
                    // Add curve based on where it hits
                    float hitX = transform.position.x - hit.collider.transform.position.x;
                    float width = hit.collider.bounds.size.x;
                    float percent = hitX / (width / 2f);
                    direction = new Vector2(percent, 1f).normalized;
                    Debug.Log("Bounced off paddle. New direction: " + direction);
                }
                else
                {
                    // Reflect based on surface normal
                    direction = Vector2.Reflect(direction, hit.normal).normalized;
                    Debug.Log("Reflected direction: " + direction);
                }

                if (tag == "Brick")
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Brick destroyed.");
                }

                // Nudge off surface to prevent sticking
                transform.position += (Vector3)(hit.normal * 0.02f);
            }
            else
            {
                // Move freely if nothing hit
                transform.position += (Vector3)move;
            }

            // Prevent stuck state with zero direction
            if (direction == Vector2.zero)
            {
                direction = Vector2.up;
                Debug.LogWarning("Direction was zero! Resetting to Vector2.up.");
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || !isLaunched) return;

        float moveDistance = speed * Time.fixedDeltaTime;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * moveDistance));
    }
}
