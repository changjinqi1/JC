using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumpback : MonoBehaviour

{
    public float upwardForce = 5f;
    private bool spaceCooldown = false;

    void Start()
    {
        Destroy(gameObject, 30f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
            }

            if (!spaceCooldown)
            {
                StartCoroutine(SpacebarCooldown());
            }
        }
    }

    private IEnumerator SpacebarCooldown()
    {
        spaceCooldown = true;
        yield return new WaitForSeconds(1.5f);
        spaceCooldown = false;
    }

    void Update()
    {
        if (spaceCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Spacebar is disabled during cooldown.");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Spacebar pressed!");
            }
        }
    }
}
