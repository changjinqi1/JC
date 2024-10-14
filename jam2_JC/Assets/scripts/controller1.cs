using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller1 : MonoBehaviour
{
    private enemyobjectspawner spawner;

    public void Initialize(enemyobjectspawner spawnerInstance)
    {
        spawner = spawnerInstance;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Table"))
        {
            spawner.StartCountdown(gameObject);
        }
    }
}