using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishmovementrestriction : MonoBehaviour

{
    public float minY = -2.9f;
    public float maxY = 2.9f;

    private void Update()
    {
        Vector3 position = transform.position;

        position.y = Mathf.Clamp(position.y, minY, maxY);

        transform.position = position;
    }
}
