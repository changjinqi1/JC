using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    [Header("QTE Settings")]
    public GameObject qteFrame;             // Reference to the QTE frame UI element

    [Header("Treasure Settings")]
    public GameObject treasurePrefab;       // Reference to the treasure box prefab
    public float treasureXPosition = -4.36f; // Fixed x position for the treasure
    public Vector2 treasureSpawnRangeY = new Vector2(-2.8f, 2.8f); // Y range for spawning

    private GameObject spawnedTreasure;     // Reference to the spawned treasure instance

    private void Update()
    {
        // Check if the QTE frame is active
        if (qteFrame.activeSelf)
        {
            // Try to spawn treasure if not already spawned
            if (spawnedTreasure == null)
            {
                Debug.Log("try to spawn treasure");
                TrySpawnTreasure();
            }
        }
        else
        {
            // Destroy treasure if QTE frame is disabled
            DestroyTreasure();
        }
    }

    private void TrySpawnTreasure()
    {
        // 30% chance to spawn the treasure box
        if (Random.value <= 0.3f)
        {
            Debug.Log("treasure is spawned");
            // Generate a random y position within the specified range
            float randomY = Random.Range(treasureSpawnRangeY.x, treasureSpawnRangeY.y);
            Vector2 spawnPosition = new Vector2(treasureXPosition, randomY);

            // Instantiate the treasure at the calculated position
            spawnedTreasure = Instantiate(treasurePrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void DestroyTreasure()
    {
        if (spawnedTreasure != null)
        {
            Destroy(spawnedTreasure);
            spawnedTreasure = null;
        }
    }
}
