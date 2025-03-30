using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float sensitivity = 5f;
    public float rotationYMin = -60f;
    public float rotationYMax = 60f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hides and locks the cursor
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX += mouseX;
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, rotationYMin, rotationYMax);

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }
}
