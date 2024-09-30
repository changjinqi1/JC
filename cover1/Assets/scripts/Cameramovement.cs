using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public Vector2 xLimits = new Vector2(-10f, 6f);
    public Vector2 zLimits = new Vector2(-2f, 12f);

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical);

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        Vector3 cameraPosition = transform.position;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, xLimits.x, xLimits.y);
        cameraPosition.z = Mathf.Clamp(cameraPosition.z, zLimits.x, zLimits.y);

        transform.position = cameraPosition;
    }
}
