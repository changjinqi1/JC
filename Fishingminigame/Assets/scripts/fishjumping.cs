using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishjumping : MonoBehaviour

{
    [Header("Time Settings")]
    public float minInterval = 0.5f;
    public float maxInterval = 6f;

    [Header("Force Settings")]
    public float minForce = 0.4f;
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

        StartCoroutine(ApplyRandomUpwardForce());
    }

    private IEnumerator ApplyRandomUpwardForce()
    {
        while (true) 
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            float randomForce = Random.Range(minForce, maxForce);

            rb.AddForce(Vector2.up * randomForce, ForceMode2D.Impulse);
        }
    }
}
