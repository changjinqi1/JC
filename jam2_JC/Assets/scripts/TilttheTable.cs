using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilttheTable : MonoBehaviour
{
    public float sensitivityX = 5f;
    public float sensitivityZ = 5f;

    private Quaternion initialRotation;
    private Vector2 screenCenter;

    void Start()
    {
        initialRotation = transform.rotation;
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        // Calculate offsets from the screen center
        float offsetX = (mousePos.x - screenCenter.x) / screenCenter.x;
        float offsetY = (mousePos.y - screenCenter.y) / screenCenter.y;

        // Convert offsets to rotation amounts
        float rotationX = offsetY * sensitivityX;
        float rotationZ = -offsetX * sensitivityZ;

        // Apply rotation based on initial rotation and mouse movement
        transform.rotation = initialRotation * Quaternion.Euler(rotationX, 0, rotationZ);
    }

    // Method to update initialRotation after manual rotation
    public void UpdateInitialRotation(Quaternion newRotation)
    {
        initialRotation = newRotation;
    }
}