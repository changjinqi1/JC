using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hooker : MonoBehaviour
{
    public GameObject hook;
    public float moveSpeed = 5f;
    public float destroyDelay = 2f;

    private Camera mainCamera;
    private float spawnDelay = 8f;
    private bool canSpawn = true;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0) && canSpawn)
        {
            // Start the spawn process
            StartCoroutine(SpawnAndMove());
        }
    }

    private IEnumerator SpawnAndMove()
    {
        canSpawn = false; // Prevent further spawning

        // Perform a raycast from the camera to the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Spawn the object at the camera's position
            GameObject spawnedObject = Instantiate(objectToSpawn, mainCamera.transform.position, Quaternion.identity);

            // Start moving the object toward the hit point
            StartCoroutine(MoveObjectTowards(spawnedObject, hit.point));
        }

        // Wait for the spawn delay before allowing another spawn
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true; // Allow spawning again
    }

    // Coroutine to move the spawned object towards the hit point and then return to the camera
    private IEnumerator MoveObjectTowards(GameObject obj, Vector3 target)
    {
        // Get the direction to the target
        Vector3 direction = (target - obj.transform.position).normalized;

        // Move the object towards the target
        while (Vector3.Distance(obj.transform.position, target) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Return immediately when hitting an object
        yield return StartCoroutine(ReturnToCamera(obj));
    }

    // Coroutine to move the object back to the camera's position
    private IEnumerator ReturnToCamera(GameObject obj)
    {
        Vector3 cameraPosition = mainCamera.transform.position;

        // Move the object back to the camera's position
        while (Vector3.Distance(obj.transform.position, cameraPosition) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, cameraPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait for 2 seconds, then destroy the object
        yield return new WaitForSeconds(destroyDelay);
        Destroy(obj);
    }
}
