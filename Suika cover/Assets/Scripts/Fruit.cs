using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fruit : MonoBehaviour
{
    public int value;  // The unique value of the fruit
    public int price;  // Price of the fruit (modifiable in Inspector)
    private FruitManager fruitManager;
    private bool isMerging = false;
    public float pushForce = 0.5f;
    public ParticleSystem mergeEffect; // Assign in Inspector
    public AudioSource mergeAudioSource; // Assign this in the Inspector

    void Start()
    {
        fruitManager = FindObjectOfType<FruitManager>();

        // Find the global AudioManager in the scene if not assigned
        if (mergeAudioSource == null)
        {
            GameObject audioManager = GameObject.Find("AudioManager");
            if (audioManager == null)
            {
                Debug.LogWarning("AudioManager not found in the scene! Make sure it exists.");
            }
            else
            {
                mergeAudioSource = audioManager.GetComponent<AudioSource>();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMerging) return; // Prevent merging if already merging

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if (otherFruit != null && !otherFruit.isMerging && otherFruit.value == this.value)
        {
            // Start the merge process with a short delay
            StartCoroutine(HandleMerge(otherFruit));
        }
    }

    private Coroutine gameOverCoroutine; // Store the coroutine reference

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boarder") && isMerging)
        {
            // Start the countdown to Game Over
            gameOverCoroutine = StartCoroutine(GameOverCountdown());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boarder") && isMerging)
        {
            // Cancel the Game Over countdown if the fruit leaves the border
            if (gameOverCoroutine != null)
            {
                StopCoroutine(gameOverCoroutine);
                gameOverCoroutine = null;
            }
        }
    }

    private IEnumerator GameOverCountdown()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        // After 2 seconds, trigger Game Over
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator HandleMerge(Fruit otherFruit)
    {
        isMerging = true;
        otherFruit.isMerging = true;

        // Apply a force to push the other object away
        Vector2 pushDirection = (otherFruit.transform.position - transform.position).normalized;
        otherFruit.GetComponent<Rigidbody2D>().AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

        // Short delay for chain reactions
        yield return new WaitForSeconds(0.1f);

        // **Play Merge Effect**
        if (mergeEffect != null)
        {
            Vector2 collisionPosition = (transform.position + otherFruit.transform.position) / 2;
            ParticleSystem effect = Instantiate(mergeEffect, collisionPosition, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration); // Destroy particle effect after playing
        }

        // **Play Merge Sound**
        if (mergeAudioSource != null)
        {
            mergeAudioSource.Play();
        }

        // Add the price of the merging fruit to the score
        GameManager.Instance.AddScore(price);

        // Handle merging
        Vector2 newPosition = (transform.position + otherFruit.transform.position) / 2;
        fruitManager.HandleFruitCollision(this, otherFruit, newPosition);
    }

    public void ResetMergingState()
    {
        isMerging = false; // Allows new merges to happen
    }
}
