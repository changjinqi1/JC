using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerBlue : MonoBehaviour

{
    public GameObject blueWall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(blueWall);
        }
    }
}
