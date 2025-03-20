using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] level1to5Fruits;
    public Transform spawnLocationA; // Location A (Next Fruit)
    public Transform playerLocation;
    public float dropCooldown = 0.7f; // Cooldown duration

    private GameObject fruitAtLocationA; // The fruit at Location A
    private GameObject fruitAtLocationB; // The fruit at Player Position
    private bool isCooldown = false; // Tracks if drop is on cooldown

    void Start()
    {
        SpawnFruitAtLocationA();
    }

    void SpawnFruitAtLocationA()
    {
        if (level1to5Fruits.Length == 0 || isCooldown) return; // Prevent spawning during cooldown

        int randomIndex = Random.Range(0, level1to5Fruits.Length);
        fruitAtLocationA = Instantiate(level1to5Fruits[randomIndex], spawnLocationA.position, Quaternion.identity);

        SetRigidbodyStatic(fruitAtLocationA);
    }

    void SetRigidbodyStatic(GameObject fruit)
    {
        Rigidbody fruitRb = fruit.GetComponent<Rigidbody>();
        Collider fruitCol = fruit.GetComponent<Collider>();

        if (fruitRb)
        {
            fruitRb.isKinematic = true;
            fruitRb.useGravity = false;
        }
        if (fruitCol)
        {
            fruitCol.enabled = false;
        }
    }

    void SetRigidbodyDynamic(GameObject fruit)
    {
        Rigidbody fruitRb = fruit.GetComponent<Rigidbody>();
        Collider fruitCol = fruit.GetComponent<Collider>();

        if (fruitRb)
        {
            fruitRb.isKinematic = false;
            fruitRb.useGravity = true;
            fruitRb.velocity = Vector3.zero; // Prevent unwanted movement
        }
        if (fruitCol)
        {
            fruitCol.enabled = true;
        }
    }

    public void DropButtonPressed()
    {
        if (isCooldown) return; // Prevent action during cooldown

        StartCoroutine(DropCooldown()); // Start cooldown

        if (fruitAtLocationB != null)
        {
            // Drop the fruit from Location B
            SetRigidbodyDynamic(fruitAtLocationB);
            fruitAtLocationB = null;
        }

        if (fruitAtLocationA != null)
        {
            // Move fruit from Location A to Player's position (without parenting)
            fruitAtLocationA.transform.position = playerLocation.position;
            fruitAtLocationB = fruitAtLocationA;
            fruitAtLocationA = null;

            // Spawn new fruit at Location A **only if not in cooldown**
            SpawnFruitAtLocationA();
        }
    }

    private IEnumerator DropCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(dropCooldown);
        isCooldown = false;

        // Spawn new fruit once cooldown ends
        if (fruitAtLocationA == null)
        {
            SpawnFruitAtLocationA();
        }
    }
}
