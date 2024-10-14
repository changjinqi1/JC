using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballmovement : MonoBehaviour

{
    public Rigidbody rb;
    public GameObject platform;

    public float forceMultiplier = 10f;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (platform == null)
        {
            Debug.LogError("Platform GameObject is not assigned! Please assign it in the Inspector.");
        }
    }

    void FixedUpdate()
    {
        if (platform != null)
        {
            Transform platformTransform = platform.transform;

            Vector3 platformTilt = platformTransform.rotation.eulerAngles;

            float tiltX = Mathf.DeltaAngle(0, platformTilt.x);
            float tiltZ = Mathf.DeltaAngle(0, platformTilt.z);

            Vector3 forceDirection = new Vector3(-tiltZ, 0, tiltX).normalized;

            rb.AddForce(forceDirection * forceMultiplier * rb.mass);
        }
    }
}