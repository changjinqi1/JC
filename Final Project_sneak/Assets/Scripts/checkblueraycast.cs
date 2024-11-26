using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckBlueRaycast : MonoBehaviour
{
    public Transform player;
    public float rayDistance = 50f;
    public int raysOnXAxis = 4;
    public int raysOnYAxis = 20;
    public GameObject particleEffectPrefab;

    private bool playerAlreadyDetected = false;

    void Update()
    {
        if (ColorChangeBlue.IsInColorB && !playerAlreadyDetected)
        {
            if (IsPlayerDetectedByBluelight())
            {
                playerAlreadyDetected = true; // Prevent multiple triggers
                HandlePlayerExposure();
            }
        }

        // Reset detection when the blue light turns off
        if (!ColorChangeBlue.IsInColorB)
        {
            playerAlreadyDetected = false;
        }
    }

    bool IsPlayerDetectedByBluelight()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is not assigned.");
            return false;
        }

        Collider blueLightCollider = GetComponent<Collider>();
        if (blueLightCollider == null)
        {
            Debug.LogWarning("Bluelight does not have a Collider component.");
            return false;
        }

        Bounds blueLightBounds = blueLightCollider.bounds;

        for (int x = 0; x < raysOnXAxis; x++)
        {
            for (int y = 0; y < raysOnYAxis; y++)
            {
                float normalizedX = (float)x / (raysOnXAxis - 1);
                float normalizedY = (float)y / (raysOnYAxis - 1);

                Vector3 pointOnBlueLight = new Vector3(
                    Mathf.Lerp(blueLightBounds.min.x, blueLightBounds.max.x, normalizedX),
                    Mathf.Lerp(blueLightBounds.min.y, blueLightBounds.max.y, normalizedY),
                    blueLightBounds.max.z
                );

                if (Physics.Raycast(pointOnBlueLight, Vector3.forward, out RaycastHit hit, rayDistance))
                {
                    if (hit.collider.transform == player)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void HandlePlayerExposure()
    {
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, player.position, Quaternion.identity);
        }

        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }

        MonoBehaviour playerMovement = player.GetComponent<MonoBehaviour>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        StartCoroutine(DestroyPlayerAfterDelay(player.gameObject));
    }

    private IEnumerator DestroyPlayerAfterDelay(GameObject player)
    {
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            playerRenderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(1.5f);

        foreach (Transform child in player.transform)
        {
            child.SetParent(null);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Collider blueLightCollider = GetComponent<Collider>();
            if (blueLightCollider == null) return;

            Bounds blueLightBounds = blueLightCollider.bounds;

            for (int x = 0; x < raysOnXAxis; x++)
            {
                for (int y = 0; y < raysOnYAxis; y++)
                {
                    float normalizedX = (float)x / (raysOnXAxis - 1);
                    float normalizedY = (float)y / (raysOnYAxis - 1);

                    Vector3 pointOnBlueLight = new Vector3(
                        Mathf.Lerp(blueLightBounds.min.x, blueLightBounds.max.x, normalizedX),
                        Mathf.Lerp(blueLightBounds.min.y, blueLightBounds.max.y, normalizedY),
                        blueLightBounds.max.z
                    );

                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(pointOnBlueLight, pointOnBlueLight + Vector3.forward * rayDistance);
                }
            }
        }
    }
}
