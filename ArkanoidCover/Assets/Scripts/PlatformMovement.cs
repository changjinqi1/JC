using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour

{
    public float speed = 5f;
    public float minX = -2f;
    public float maxX = 2f;

    void FixedUpdate()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
        }

        // Calculate new position
        float newX = transform.position.x + moveInput * speed * Time.deltaTime;

        // Clamp to boundaries
        newX = Mathf.Clamp(newX, minX, maxX);

        // Apply the movement
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
