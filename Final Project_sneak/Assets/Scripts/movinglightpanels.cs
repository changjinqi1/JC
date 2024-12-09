using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movinglightpanels : MonoBehaviour

{
    public float upperLimit = 95f;
    public float lowerLimit = -10f;
    public float speed = 2f;

    private bool movingUp = true;

    void Update()
    {
        Vector3 currentPosition = transform.position;

        if (movingUp)
        {
            currentPosition.y += speed * Time.deltaTime;
            if (currentPosition.y >= upperLimit)
            {
                currentPosition.y = upperLimit;
                movingUp = false;
            }
        }
        else
        {
            currentPosition.y -= speed * Time.deltaTime;
            if (currentPosition.y <= lowerLimit)
            {
                currentPosition.y = lowerLimit;
                movingUp = true;
            }
        }

        transform.position = currentPosition;
    }
}
