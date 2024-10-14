using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugd : MonoBehaviour
{
    void Update()
    {
        if (!gameObject.activeSelf)
        {
            Debug.Log("The object is disabled: " + gameObject.name);
        }
    }
}
