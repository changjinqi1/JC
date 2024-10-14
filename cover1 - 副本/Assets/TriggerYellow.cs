using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerYellow : MonoBehaviour
{
    public GameObject yellowWall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(yellowWall);
        }
    }
}