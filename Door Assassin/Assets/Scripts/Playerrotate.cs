using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerrotate : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 5f;

    void Update()
    {
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f; // Only rotate on Y axis
        Quaternion targetRotation = Quaternion.LookRotation(camForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}