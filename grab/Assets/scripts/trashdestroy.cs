using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashdestroy : MonoBehaviour
{
    private bool isCollidingWithPlayer = false;
    private float collisionTime = 0f;
    public float requiredCollisionTime = 2f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
            collisionTime = 0f;
        }
    }

    void Update()
    {
        if (isCollidingWithPlayer)
        {
            collisionTime += Time.deltaTime;

            if (collisionTime >= requiredCollisionTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
