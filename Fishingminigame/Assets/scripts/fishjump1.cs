using System.Collections;
using UnityEngine;

public class FishJump : MonoBehaviour
{
    [Header("Time Settings")]
    public float minInterval = 0.5f;
    public float maxInterval = 2f;

    [Header("Force Settings")]
    public float minForce = 0.5f;
    public float maxForce = 4f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D component found.");
            return;
        }

        // Start the coroutine for applying random forces
        StartCoroutine(ApplyRandomForce());
    }

    private IEnumerator ApplyRandomForce()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Generate a random force between minForce and maxForce
            float randomForce = Random.Range(minForce, maxForce);

            // Randomly decide up/down
            Vector2 forceDirection = Random.value > 0.5f ? Vector2.up : Vector2.down;

            // Apply the force to the Rigidbody
            rb.AddForce(forceDirection * randomForce, ForceMode2D.Impulse);
        }
    }
}
