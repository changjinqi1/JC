using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckGreenRaycast : MonoBehaviour
{
    public Transform player;
    public float rayDistance = 50f;
    public int raysOnXAxis = 4;
    public int raysOnYAxis = 20;
    public GameObject particleEffectPrefab;

    private bool playerAlreadyDetected = false;

    void Update()
    {
        if (ColorChangeGreenLight.IsInColorC && !playerAlreadyDetected && IsPlayerDetectedByGreenlight())
        {
            playerAlreadyDetected = true;
            HandlePlayerExposure();
        }

        // Reset detection when the green light turns off
        if (!ColorChangeGreenLight.IsInColorC)
        {
            playerAlreadyDetected = false;
        }
    }

    bool IsPlayerDetectedByGreenlight()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is not assigned.");
            return false;
        }

        Collider greenLightCollider = GetComponent<Collider>();
        if (greenLightCollider == null)
        {
            Debug.LogWarning("Greenlight does not have a Collider component.");
            return false;
        }

        Bounds greenLightBounds = greenLightCollider.bounds;

        for (int x = 0; x < raysOnXAxis; x++)
        {
            for (int y = 0; y < raysOnYAxis; y++)
            {
                float normalizedX = (float)x / (raysOnXAxis - 1);
                float normalizedY = (float)y / (raysOnYAxis - 1);

                Vector3 pointOnGreenLight = new Vector3(
                    Mathf.Lerp(greenLightBounds.min.x, greenLightBounds.max.x, normalizedX),
                    Mathf.Lerp(greenLightBounds.min.y, greenLightBounds.max.y, normalizedY),
                    greenLightBounds.min.z // Back surface of the Greenlight
                );

                if (Physics.Raycast(pointOnGreenLight, Vector3.back, out RaycastHit hit, rayDistance))
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
            Collider greenLightCollider = GetComponent<Collider>();
            if (greenLightCollider == null) return;

            Bounds greenLightBounds = greenLightCollider.bounds;

            for (int x = 0; x < raysOnXAxis; x++)
            {
                for (int y = 0; y < raysOnYAxis; y++)
                {
                    float normalizedX = (float)x / (raysOnXAxis - 1);
                    float normalizedY = (float)y / (raysOnYAxis - 1);

                    Vector3 pointOnGreenLight = new Vector3(
                        Mathf.Lerp(greenLightBounds.min.x, greenLightBounds.max.x, normalizedX),
                        Mathf.Lerp(greenLightBounds.min.y, greenLightBounds.max.y, normalizedY),
                        greenLightBounds.min.z
                    );

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(pointOnGreenLight, pointOnGreenLight + Vector3.back * rayDistance);
                }
            }
        }
    }
}
