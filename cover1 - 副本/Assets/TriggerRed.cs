using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRed : MonoBehaviour
{
    public GameObject redWall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(redWall);
        }
    }
}