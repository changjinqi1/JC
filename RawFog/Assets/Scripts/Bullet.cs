using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public LineRenderer trajectoryRenderer;

    private Vector2[] trajectoryPoints;
    private int currentPointIndex = 0;
    private Vector2 endPosition;
    private Rigidbody2D rb;
    private Collider2D bulletCollider;
    private bool hitPlayer = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Ensure Rigidbody is assigned in runtime
        bulletCollider = GetComponent<Collider2D>(); // Ensure Collider is assigned
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get sprite renderer
    }

    public void SetTrajectory(Vector2 startPos, Vector2 playerPos, Vector2 endPos)
    {
        transform.position = startPos;

        if (trajectoryRenderer == null)
        {
            trajectoryRenderer = GetComponent<LineRenderer>(); // Ensure LineRenderer exists
            if (trajectoryRenderer == null)
            {
                trajectoryRenderer = gameObject.AddComponent<LineRenderer>(); // Add if missing
            }
        }

        trajectoryRenderer.useWorldSpace = true;
        endPosition = GetOffScreenPosition(startPos, playerPos, endPos);

        GenerateStraightTrajectory(startPos, playerPos, endPosition);
        StartCoroutine(FollowTrajectory());
    }

    void GenerateStraightTrajectory(Vector2 startPos, Vector2 playerPos, Vector2 endPos)
    {
        int pointCount = 3;
        trajectoryRenderer.positionCount = pointCount;
        trajectoryPoints = new Vector2[pointCount];

        trajectoryPoints[0] = startPos;
        trajectoryPoints[1] = playerPos;
        trajectoryPoints[2] = endPos;

        Vector3[] points3D = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            points3D[i] = new Vector3(trajectoryPoints[i].x, trajectoryPoints[i].y, 0);
        }
        trajectoryRenderer.SetPositions(points3D);
    }

    IEnumerator FollowTrajectory()
    {
        yield return new WaitForSeconds(1f); // Delay before movement

        while (currentPointIndex < trajectoryPoints.Length && !hitPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, trajectoryPoints[currentPointIndex], speed * Time.deltaTime);
            UpdateLineRenderer();

            if ((Vector2)transform.position == trajectoryPoints[currentPointIndex])
            {
                currentPointIndex++;
            }

            if (currentPointIndex >= trajectoryPoints.Length)
            {
                DestroyBullet();
                yield break;
            }

            yield return null;
        }
    }

    void UpdateLineRenderer()
    {
        if (trajectoryRenderer != null && currentPointIndex < trajectoryPoints.Length - 1)
        {
            trajectoryRenderer.SetPosition(0, transform.position);
            for (int i = 1; i < trajectoryRenderer.positionCount; i++)
            {
                trajectoryRenderer.SetPosition(i, trajectoryPoints[i]);
            }
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    Vector2 GetOffScreenPosition(Vector2 startPos, Vector2 playerPos, Vector2 originalEndPos)
    {
        Camera mainCamera = Camera.main;
        float screenHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float screenHalfHeight = mainCamera.orthographicSize;

        Vector2 direction = (originalEndPos - startPos).normalized;
        float offScreenDistance = Mathf.Max(screenHalfWidth, screenHalfHeight) * 2f;

        return playerPos + direction * offScreenDistance;
    }

    // Handle collision with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hitPlayer)
        {
            StartCoroutine(HitPlayerEffect());
        }
    }

    IEnumerator HitPlayerEffect()
    {
        hitPlayer = true;

        // Stop movement and disable collider
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if (bulletCollider != null)
        {
            bulletCollider.enabled = false;
        }

        // Change bullet color to red
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        // Change trajectory color to red
        if (trajectoryRenderer != null)
        {
            trajectoryRenderer.startColor = Color.red;
            trajectoryRenderer.endColor = Color.red;
        }

        // Wait for 0.5 seconds before destroying
        yield return new WaitForSeconds(0.5f);
        DestroyBullet();
    }
}
