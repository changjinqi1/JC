using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stick : MonoBehaviour
{
    public GameObject parentObject;

    void Start()
    {
        if (parentObject != null)
        {
            transform.SetParent(parentObject.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
