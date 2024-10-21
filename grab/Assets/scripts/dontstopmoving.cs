using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontstopmoving : MonoBehaviour

{
    public Rigidbody rb; 
    public float stopThreshold = 1f;
    public float pushForce = 0.5f;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (rb.velocity.magnitude < stopThreshold)
        {
            rb.AddForce(Vector3.left * pushForce, ForceMode.Impulse);
        }
    }
}
