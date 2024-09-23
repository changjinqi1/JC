using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plmovement : MonoBehaviour
{
    public float rotationSpeed = 200f;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
    }
}