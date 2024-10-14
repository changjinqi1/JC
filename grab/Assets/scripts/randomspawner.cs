using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomspawner : MonoBehaviour

{
    public GameObject[] objectsToSpawn;

    private Vector3[] spawnLocations = new Vector3[]
    {
        new Vector3(25f, 16f, 8f),
        new Vector3(25f, 17f, 3.4f),
        new Vector3(25f, 17f, -3f),
        new Vector3(25f, 17f, -8f)
    };

    public float spawnInterval = 3f;

    void Start()
    {
        InvokeRepeating("SpawnRandomObject", 0f, spawnInterval);
    }

    void SpawnRandomObject()
    {
        int randomObjectIndex = Random.Range(0, objectsToSpawn.Length);

        int randomLocationIndex = Random.Range(0, spawnLocations.Length);

        Instantiate(objectsToSpawn[randomObjectIndex], spawnLocations[randomLocationIndex], Quaternion.identity);
    }
}
