using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speed = 5f;
    public LineRenderer trajectoryRenderer;

    private Vector2[] trajectoryPoints;
    private int currentPointIndex = 0;
    private Vector2 endPosition;
    private bool isOffScreen = false;

    public void SetTrajectory(Vector2 startPos, Vector2 playerPos, Vector2 endPos)
    {
        transform.position = startPos;

        if (trajectoryRenderer != null)
        {
            trajectoryRenderer.useWorldSpace = true;
        }

        // Ensure endPos is off-screen
        endPosition = GetOffScreenPosition(startPos, playerPos, endPos);

        GenerateStraightTrajectory(startPos, playerPos, endPosition);
        StartCoroutine(FollowTrajectory());
    }

    void GenerateStraightTrajectory(Vector2 startPos, Vector2 playerPos, Vector2 endPos)
    {
        int pointCount = 3; // Start, Player, Off-Screen End
        trajectoryRenderer.positionCount = pointCount;
        trajectoryPoints = new Vector2[pointCount];

        trajectoryPoints[0] = startPos;
        trajectoryPoints[1] = playerPos; // Ensure it passes near the player
        trajectoryPoints[2] = endPos; // End position off-screen

        Vector3[] points3D = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            points3D[i] = new Vector3(trajectoryPoints[i].x, trajectoryPoints[i].y, 0);
        }
        trajectoryRenderer.SetPositions(points3D);

        // Disable Rigidbody (Ensure physics is OFF)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Destroy(rb); // Remove Rigidbody to prevent physics interference
        }

        // Ensure the power-up has a trigger collider for collision detection
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true; // Make sure it doesn't use physics collisions
        }
    }

    IEnumerator FollowTrajectory()
    {
        while (currentPointIndex < trajectoryPoints.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position, trajectoryPoints[currentPointIndex], speed * Time.deltaTime);

            // Update LineRenderer to show only the remaining trajectory
            UpdateLineRenderer();

            if ((Vector2)transform.position == trajectoryPoints[currentPointIndex])
            {
                currentPointIndex++;
            }

            // Check if off-screen
            if (IsOffScreen())
            {
                if (!isOffScreen)
                {
                    isOffScreen = true;
                    StartCoroutine(DestroyIfOffScreenTooLong()); // Start countdown to destroy
                }
            }
            else
            {
                isOffScreen = false; // Reset if it's back on screen
            }

            // If power-up reaches the final off-screen position, destroy it
            if (currentPointIndex >= trajectoryPoints.Length)
            {
                DestroyPowerUp();
                yield break;
            }

            yield return null;
        }
    }

    void UpdateLineRenderer()
    {
        if (trajectoryRenderer != null && currentPointIndex < trajectoryPoints.Length - 1)
        {
            // Set the start position to the power-up's current position
            trajectoryRenderer.SetPosition(0, transform.position);

            // Keep the rest of the trajectory unchanged
            for (int i = 1; i < trajectoryRenderer.positionCount; i++)
            {
                trajectoryRenderer.SetPosition(i, trajectoryPoints[i]);
            }
        }
    }

    void DestroyPowerUp()
    {
        Destroy(gameObject);
        if (trajectoryRenderer != null)
        {
            Destroy(trajectoryRenderer.gameObject);
        }
    }

    // Ensure the power-up is destroyed when hitting the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyPowerUp();
        }
    }

    // Check if the power-up is off the screen
    bool IsOffScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }

    // Destroy power-up if off-screen for 2 seconds
    IEnumerator DestroyIfOffScreenTooLong()
    {
        yield return new WaitForSeconds(2f);
        if (IsOffScreen())
        {
            DestroyPowerUp();
        }
    }

    Vector2 GetOffScreenPosition(Vector2 startPos, Vector2 playerPos, Vector2 originalEndPos)
    {
        Camera mainCamera = Camera.main;
        float screenHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float screenHalfHeight = mainCamera.orthographicSize;

        Vector2 direction = (originalEndPos - startPos).normalized;
        float offScreenDistance = Mathf.Max(screenHalfWidth, screenHalfHeight) * 2f; // Ensure it's far enough

        return playerPos + direction * offScreenDistance; // Move beyond screen bounds
    }
}
