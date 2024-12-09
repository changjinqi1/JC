using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomScroll : MonoBehaviour

{
    public Camera targetCamera;
    public float zoomSpeed = 10f;
    public float minFoV = 15f;
    public float maxFoV = 90f;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float newFoV = targetCamera.fieldOfView - scrollInput * zoomSpeed;
            targetCamera.fieldOfView = Mathf.Clamp(newFoV, minFoV, maxFoV);
        }
    }
}
