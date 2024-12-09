using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckOrangeRaycast : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float rayDistance = 50f;
    [SerializeField] private int raysOnXAxis = 4;
    [SerializeField] private int raysOnZAxis = 4;
    [SerializeField] private GameObject particleEffectPrefab;

    void Update()
    {
        // Check if the ColorChangeOrange object is in the orange color state
        if (ColorChangeOrange.IsInColorO)
        {
            if (IsPlayerDetectedByOrangelight())
            {
                HandlePlayerExposure();
            }
        }
    }

    private bool IsPlayerDetectedByOrangelight()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is not assigned.");
            return false;
        }

        Collider orangeLightCollider = GetComponent<Collider>();
        if (orangeLightCollider == null)
        {
            Debug.LogWarning("The object does not have a Collider component.");
            return false;
        }

        Bounds orangeLightBounds = orangeLightCollider.bounds;

        for (int x = 0; x < raysOnXAxis; x++)
        {
            for (int z = 0; z < raysOnZAxis; z++)
            {
                float normalizedX = (float)x / (raysOnXAxis - 1);
                float normalizedZ = (float)z / (raysOnZAxis - 1);

                Vector3 pointOnOrangeLight = new Vector3(
                    Mathf.Lerp(orangeLightBounds.min.x, orangeLightBounds.max.x, normalizedX),
                    orangeLightBounds.max.y, // Start from the top of the object
                    Mathf.Lerp(orangeLightBounds.min.z, orangeLightBounds.max.z, normalizedZ)
                );

                if (Physics.Raycast(pointOnOrangeLight, Vector3.down, out RaycastHit hit, rayDistance))
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

    private void HandlePlayerExposure()
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

        // Disable all movement or scripts on the player
        MonoBehaviour[] playerScripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in playerScripts)
        {
            script.enabled = false;
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
            Collider orangeLightCollider = GetComponent<Collider>();
            if (orangeLightCollider == null) return;

            Bounds orangeLightBounds = orangeLightCollider.bounds;

            for (int x = 0; x < raysOnXAxis; x++)
            {
                for (int z = 0; z < raysOnZAxis; z++)
                {
                    float normalizedX = (float)x / (raysOnXAxis - 1);
                    float normalizedZ = (float)z / (raysOnZAxis - 1);

                    Vector3 pointOnOrangeLight = new Vector3(
                        Mathf.Lerp(orangeLightBounds.min.x, orangeLightBounds.max.x, normalizedX),
                        orangeLightBounds.max.y, // Start from the top of the object
                        Mathf.Lerp(orangeLightBounds.min.z, orangeLightBounds.max.z, normalizedZ)
                    );

                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(pointOnOrangeLight, pointOnOrangeLight + Vector3.down * rayDistance);
                }
            }
        }
    }
}
