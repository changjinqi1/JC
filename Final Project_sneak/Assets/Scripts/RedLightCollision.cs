using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedLightCollision : MonoBehaviour
{
    public GameObject particleEffectPrefab;
    public float destroyDelay = 1f;
    public float redColorDuration = 1.5f;

    private bool isProcessingCollision = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ColorChangeandCollisionRed.IsInColorA)
            {
                Debug.Log("RedLight is active, and player has entered the collision.");
                ProcessPlayerCollision(other.gameObject);
            }
            else
            {
                Debug.Log("RedLight is NOT active, no action taken.");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && ColorChangeandCollisionRed.IsInColorA && !isProcessingCollision)
        {
            Debug.Log("Player is staying in the RedLight while it is active.");
            ProcessPlayerCollision(other.gameObject);
        }
    }

    private void ProcessPlayerCollision(GameObject player)
    {
        if (isProcessingCollision) return;

        isProcessingCollision = true;

        // Instantiate particle effect
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, player.transform.position, Quaternion.identity);
        }

        // Freeze player movement
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }
        else
        {
            Debug.LogWarning("Player Rigidbody not found!");
        }

        // Disable player movement script
        MonoBehaviour playerMovement = player.GetComponent<MonoBehaviour>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        else
        {
            Debug.LogWarning("Player movement script not found!");
        }

        StartCoroutine(DestroyPlayerAfterDelay(player));
    }

    private IEnumerator DestroyPlayerAfterDelay(GameObject player)
    {
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            playerRenderer.material.color = Color.red;
        }
        else
        {
            Debug.LogWarning("Player Renderer not found!");
        }

        yield return new WaitForSeconds(redColorDuration);

        foreach (Transform child in player.transform)
        {
            child.SetParent(null);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        isProcessingCollision = false;
    }
}
