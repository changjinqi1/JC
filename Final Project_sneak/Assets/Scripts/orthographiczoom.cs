using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orthographiczoom : MonoBehaviour

{
    public Camera targetCamera;
    public float zoomSpeed = 2f;
    public float minSize = 2f;
    public float maxSize = 20f;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (!targetCamera.orthographic)
        {
            Debug.LogWarning("The assigned camera is not orthographic. Please ensure it's set to Orthographic mode.");
        }
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            float newSize = targetCamera.orthographicSize - scrollInput * zoomSpeed;
            targetCamera.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
        }
    }
}
