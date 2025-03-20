using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverScene : MonoBehaviour

{
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit")) // Ensure fruits have the "Fruit" tag
        {
            // Start the countdown coroutine and store it in the dictionary
            if (!activeCoroutines.ContainsKey(collision.gameObject))
            {
                Coroutine gameOverCoroutine = StartCoroutine(GameOverCountdown(collision.gameObject));
                activeCoroutines[collision.gameObject] = gameOverCoroutine;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            // Stop the countdown if the fruit exits before 1 second
            if (activeCoroutines.ContainsKey(collision.gameObject))
            {
                StopCoroutine(activeCoroutines[collision.gameObject]);
                activeCoroutines.Remove(collision.gameObject);
            }
        }
    }

    private IEnumerator GameOverCountdown(GameObject fruit)
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second

        // If the fruit is still in the trigger area, trigger Game Over
        if (fruit != null)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
