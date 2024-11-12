using UnityEngine;
using UnityEngine.UI;

public class CollisionDetection : MonoBehaviour
{
    [Header("Progress Bar Settings")]
    public Slider progressBar;
    public float increaseRate = 0.2f;
    public float decreaseRate = 0.001f;
    public float dropAcceleration = 0.001f;
    public float maxDropSpeed = 0.2f;

    private bool isCollidingWithFish = false;
    private bool isCollidingWithTreasure = false;
    private float currentDecreaseRate;
    private float treasureCollisionTimer = 0f;
    private const float treasureCollisionThreshold = 3f;

    private void Start()
    {
        if (progressBar == null)
        {
            Debug.LogError("Progress bar is not assigned. Please assign a Slider component to progressBar.");
        }
    }

    private void Update()
    {
        if (isCollidingWithTreasure)
        {
            treasureCollisionTimer += Time.deltaTime;

            if (treasureCollisionTimer >= treasureCollisionThreshold)
            {
                maxDropSpeed = 0.04f;
            }

            return;
        }
        else
        {
            treasureCollisionTimer = 0f;
        }

        if (isCollidingWithFish && progressBar != null)
        {
            progressBar.value += increaseRate * Time.deltaTime;
            progressBar.value = Mathf.Clamp(progressBar.value, progressBar.minValue, progressBar.maxValue);
        }
        else if (!isCollidingWithFish && progressBar != null)
        {
            currentDecreaseRate = Mathf.Min(currentDecreaseRate + dropAcceleration * Time.deltaTime, maxDropSpeed);

            progressBar.value -= currentDecreaseRate * Time.deltaTime;
            progressBar.value = Mathf.Clamp(progressBar.value, progressBar.minValue, progressBar.maxValue);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("fish"))
        {
            isCollidingWithFish = true;
            currentDecreaseRate = decreaseRate;
            Debug.Log("QTE Bar has started colliding with the fish.");
        }
        if (other.CompareTag("treasure"))
        {
            isCollidingWithTreasure = true;
            Debug.Log("QTE Bar has started colliding with the treasure.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("fish"))
        {
            isCollidingWithFish = false;
            Debug.Log("QTE Bar has stopped colliding with the fish.");
        }
        if (other.CompareTag("treasure"))
        {
            isCollidingWithTreasure = false;
            treasureCollisionTimer = 0f;
            maxDropSpeed = 0.15f;
            Debug.Log("QTE Bar has stopped colliding with the treasure.");
        }
    }
}
