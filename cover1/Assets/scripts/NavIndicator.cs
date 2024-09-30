using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavIndicator : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float objectLifetime = 1.0f;
    private GameObject currentSpawnedObject;

    void Start()
    {
        this.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return;
                }

                Vector3 spawnPosition = hit.point;

                if (currentSpawnedObject != null)
                {
                    currentSpawnedObject.SetActive(false);
                }


                currentSpawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

                currentSpawnedObject.SetActive(true);

                Destroy(currentSpawnedObject, objectLifetime);
            }
        }
    }
}