using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruitphysics : MonoBehaviour

{
    private Rigidbody2D rb;
    private bool isDropped = false;
    private bool playerInTrigger = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.simulated = false; // Disable physics initially
        }
    }

    void Update()
    {
        // If the fruit has been dropped and the player is in the trigger, enable physics
        if (isDropped && playerInTrigger)
        {
            EnablePhysics();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    public void DropFruit()
    {
        isDropped = true;
    }

    private void EnablePhysics()
    {
        if (rb)
        {
            rb.simulated = true;
        }
    }
}
