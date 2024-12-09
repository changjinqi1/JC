using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeJointControl : MonoBehaviour

{
    public float rotationSpeed = 10f;
    public float maxRotationSpeed = 10f;

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        float rotationAmount = 0f;

        if (Input.GetMouseButton(0))
        {
            rotationAmount = rotationSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButton(1))
        {
            rotationAmount = -rotationSpeed * Time.deltaTime;
        }

        rotationAmount = Mathf.Clamp(rotationAmount, -maxRotationSpeed * Time.deltaTime, maxRotationSpeed * Time.deltaTime);

        transform.Rotate(rotationAmount, 0, 0);
    }
}

