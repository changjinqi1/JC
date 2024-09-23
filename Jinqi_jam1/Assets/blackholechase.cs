using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackholechase : MonoBehaviour
{
    public float speed = 2f; 

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
