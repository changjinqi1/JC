using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabber : MonoBehaviour

{
    private GameObject currentObject;

    void OnTriggerEnter(Collider other)
    {
        if (currentObject == null &&
            (other.CompareTag("gc") ||
             other.CompareTag("rc") ||
             other.CompareTag("gt") ||
             other.CompareTag("rt") ||
             other.CompareTag("trash")))
        {
            currentObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentObject)
        {
            currentObject = null;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && currentObject != null)
        {
            TransformObjectToGrabberCenter(currentObject);
        }

        if (Input.GetKeyUp(KeyCode.Space) && currentObject != null)
        {
            ReleaseObject(currentObject);
        }
    }

    void TransformObjectToGrabberCenter(GameObject obj)
    {
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
    }

    void ReleaseObject(GameObject obj)
    {
        currentObject = null;
    }
}
