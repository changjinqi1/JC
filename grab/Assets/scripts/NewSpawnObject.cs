using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewSpawnObject : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;
    public GameObject objectC;
    public GameObject objectD;
    public GameObject objectE;
    public GameObject objectF;

    public TextMeshProUGUI scoreText;
    private int playerScore;

    private Vector3[] spawnLocations = new Vector3[]
    {
        new Vector3(25f, 16f, 8f), // location1 red
        new Vector3(25f, 17f, -8f) // location2 green
    };

    private int objectABCount = 0;
    private int totalSpawnCount = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (int.TryParse(scoreText.text, out int parsedScore))
        {
            playerScore = parsedScore;
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float spawnInterval = Random.Range(2f, 6f); // Default spawn interval

            if (playerScore > 5)
            {
                spawnInterval = Random.Range(1.5f, 3f); // interval when player score > 5
            }

            yield return new WaitForSeconds(spawnInterval);

            if (playerScore > 5)
            {
                // Spawn object E or F every 3 items when score > 5
                if (totalSpawnCount % 3 == 0 && totalSpawnCount != 0)
                {
                    SpawnObject(Random.Range(0, 2) == 0 ? objectE : objectF); 
                }
                else if (objectABCount >= 2 && objectABCount <= 6)
                {
                    // Spawn object C/D after 2-6 items A/B
                    SpawnObject(Random.Range(0, 2) == 0 ? objectC : objectD);
                    objectABCount = 0;
                }
                else
                {
                    SpawnObject(Random.Range(0, 2) == 0 ? objectA : objectB);
                    objectABCount++;
                }
            }
            else
            {
                // Normal spawn conditions when score <= 5
                if (totalSpawnCount % 4 == 0 && totalSpawnCount != 0)
                {
                    SpawnObject(Random.Range(0, 2) == 0 ? objectE : objectF); 
                }
                else if (objectABCount >= 4 && objectABCount <= 8)
                {
                    // Spawn object C/D after 4-8 items A/B
                    SpawnObject(Random.Range(0, 2) == 0 ? objectC : objectD);
                    objectABCount = 0;
                }
                else
                {
                    SpawnObject(Random.Range(0, 2) == 0 ? objectA : objectB);
                    objectABCount++;
                }
            }

            totalSpawnCount++;
        }
    }

    void SpawnObject(GameObject objectToSpawn)
    {
        int randomLocationIndex = 0;

        // Logic for weighted spawning locations
        if (objectToSpawn == objectA || objectToSpawn == objectC)
        {
            randomLocationIndex = Random.Range(0f, 1f) < 0.4f ? 1 : 0; // 50% chance for location2 green
        }
        else if (objectToSpawn == objectB || objectToSpawn == objectD)
        {
            randomLocationIndex = Random.Range(0f, 1f) < 0.4f ? 0 : 1; // 50% chance for location1 red
        }
        else
        {
            randomLocationIndex = Random.Range(0, spawnLocations.Length);
        }

        Instantiate(objectToSpawn, spawnLocations[randomLocationIndex], Quaternion.identity);
    }
}
