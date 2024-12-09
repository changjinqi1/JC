using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPanel2 : MonoBehaviour

{
    public float forwardLimit = 80f;
    public float backwardLimit = -20f;
    public float speed = 3f;

    private bool movingForward = true;

    void Update()
    {
        Vector3 currentPosition = transform.position;

        if (movingForward)
        {
            currentPosition.z += speed * Time.deltaTime;
            if (currentPosition.z >= forwardLimit)
            {
                currentPosition.z = forwardLimit;
                movingForward = false;
            }
        }
        else
        {
            currentPosition.z -= speed * Time.deltaTime;
            if (currentPosition.z <= backwardLimit)
            {
                currentPosition.z = backwardLimit;
                movingForward = true;
            }
        }

        transform.position = currentPosition;
    }
}
