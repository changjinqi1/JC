using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        HandleMouseDrag();
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;
            float rotationX = -deltaMouse.y * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, rotationX, Space.World);
            lastMousePosition = Input.mousePosition;
        }
    }
}
