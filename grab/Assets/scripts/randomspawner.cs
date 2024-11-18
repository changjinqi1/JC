using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomspawner : MonoBehaviour

{
    public GameObject[] itemPrefabs;
    public Transform location1;
    public Transform location2;

    private float gameTime;

    void Start()
    {
        gameTime = 0f;
        StartCoroutine(SpawnItems());
    }

    void Update()
    {
        gameTime += Time.deltaTime;
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            float spawnInterval = GetSpawnInterval();
            yield return new WaitForSeconds(spawnInterval);

            SpawnRandomItem();
        }
    }

    float GetSpawnInterval()
    {
        if (gameTime < 5f)
        {
            return Mathf.Infinity;
        }
        else if (gameTime < 15f)
        {
            return Random.Range(6f, 10f);
        }
        else if (gameTime < 25f)
        {
            return Random.Range(4f, 8f);
        }
        else
        {
            return Random.Range(2f, 5f);
        }
    }

    void SpawnRandomItem()
    {
        if (itemPrefabs.Length == 0)
        {
            Debug.LogWarning("No item prefabs assigned.");
            return;
        }

        GameObject randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        Transform randomLocation = Random.Range(0, 2) == 0 ? location1 : location2;

        Instantiate(randomItem, randomLocation.position, Quaternion.identity);
    }
}
