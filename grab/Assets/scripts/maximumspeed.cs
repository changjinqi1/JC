using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maximumspeed : MonoBehaviour

{
    public float maxSpeed = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to this GameObject.");
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }
}
