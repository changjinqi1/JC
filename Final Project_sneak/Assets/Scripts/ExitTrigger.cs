using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Renderer playerRenderer = other.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                playerRenderer.material.color = Color.green;
            }

            other.transform.position = transform.position;

            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("Scene name is not set in the Inspector.");
            }
        }
    }
}
