using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomin : MonoBehaviour

{
    public Camera targetCamera;
    public float startFoV = 153f;
    public float endFoV = 70f;
    public float duration = 1f;

    private float elapsedTime = 0f;
    private bool isZooming = false;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        targetCamera.fieldOfView = startFoV;
        StartZoom();
    }

    public void StartZoom()
    {
        isZooming = true;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (isZooming)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            targetCamera.fieldOfView = Mathf.Lerp(startFoV, endFoV, t);

            if (t >= 1f)
            {
                isZooming = false;
            }
        }
    }
}
