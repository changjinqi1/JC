using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Checkfrombehind : MonoBehaviour

{
    public Transform player;
    public float rayDistance = 50f;
    public int raysOnXAxis = 20;
    public int raysOnYAxis = 4;
    public GameObject particleEffectPrefab;

    private bool playerAlreadyDetected = false;

    void Update()
    {
        if (ColorChangeGreenLight.IsInColorC && !playerAlreadyDetected && IsPlayerDetectedByGreenlight())
        {
            playerAlreadyDetected = true;
            HandlePlayerExposure();
        }

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

        for (int y = 0; y < raysOnYAxis; y++)
        {
            for (int z = 0; z < raysOnXAxis; z++)
            {
                float normalizedY = (float)y / (raysOnYAxis - 1);
                float normalizedZ = (float)z / (raysOnXAxis - 1);

                Vector3 pointOnGreenLight = new Vector3(
                    greenLightBounds.min.x, // Left surface of the Greenlight
                    Mathf.Lerp(greenLightBounds.min.y, greenLightBounds.max.y, normalizedY),
                    Mathf.Lerp(greenLightBounds.min.z, greenLightBounds.max.z, normalizedZ)
                );

                if (Physics.Raycast(pointOnGreenLight, Vector3.right, out RaycastHit hit, rayDistance))
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

            for (int y = 0; y < raysOnYAxis; y++)
            {
                for (int z = 0; z < raysOnXAxis; z++)
                {
                    float normalizedY = (float)y / (raysOnYAxis - 1);
                    float normalizedZ = (float)z / (raysOnXAxis - 1);

                    Vector3 pointOnGreenLight = new Vector3(
                        greenLightBounds.min.x,
                        Mathf.Lerp(greenLightBounds.min.y, greenLightBounds.max.y, normalizedY),
                        Mathf.Lerp(greenLightBounds.min.z, greenLightBounds.max.z, normalizedZ)
                    );

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(pointOnGreenLight, pointOnGreenLight + Vector3.right * rayDistance);
                }
            }
        }
    }
}
