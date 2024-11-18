using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGrabber : MonoBehaviour
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
        if (Input.GetMouseButton(0) && currentObject != null)
        {
            TransformObjectToGrabberCenter1(currentObject);
        }

        if (Input.GetMouseButtonUp(0) && currentObject != null)
        {
            ReleaseObject1(currentObject);
        }
    }

    void TransformObjectToGrabberCenter1(GameObject obj)
    {
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
    }

    void ReleaseObject1(GameObject obj)
    {
        currentObject = null;
    }
}

