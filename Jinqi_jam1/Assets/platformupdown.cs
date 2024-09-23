using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformupdown : MonoBehaviour
{
    public float speed = 1f;
    public float height = 2f;

    private Vector3 originalPosition;

    void Start()
    {

        originalPosition = transform.position;
    }

    void Update()
    {

        float newY = Mathf.Sin(Time.time * speed) * height;

        transform.position = new Vector3(originalPosition.x, originalPosition.y + newY, originalPosition.z);
    }
}