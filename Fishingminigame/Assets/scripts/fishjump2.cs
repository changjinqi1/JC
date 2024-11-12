using System.Collections;
using UnityEngine;

public class FishJump2 : MonoBehaviour
{
    [Header("Time Settings")]
    public float minInterval = 2f;
    public float maxInterval = 4f;
    public float moveSpeed = 4f; 

    [Header("Movement Range")]
    public float xPosition = -4.36f;
    public float yMin = -3f;
    public float yMax = 3f;

    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        StartCoroutine(MoveToRandomPosition());
    }

    private IEnumerator MoveToRandomPosition()
    {
        while (true)
        {
            // Wait for a random interval before choosing a new target
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            float randomY = Random.Range(yMin, yMax);
            targetPosition = new Vector2(xPosition, randomY);

            // Move towards the target position
            isMoving = true;

            while (isMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Check if the fish has reached the target
                if ((Vector2)transform.position == targetPosition)
                {
                    isMoving = false;
                }

                yield return null;
            }
        }
    }
}
