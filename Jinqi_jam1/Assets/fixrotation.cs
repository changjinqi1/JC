 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixrotation : MonoBehaviour

{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

