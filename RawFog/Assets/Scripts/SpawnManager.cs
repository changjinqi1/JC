using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour

{
    public GameObject bulletSpawner;
    public GameObject bulletSpawner1;  

    void Start()
    {
        StartCoroutine(ManageSpawners());
    }

    IEnumerator ManageSpawners()
    {
        while (true)
        {
            // Enable BulletSpawner, Disable BulletSpawner1
            bulletSpawner.SetActive(true);
            bulletSpawner1.SetActive(false);
            yield return new WaitForSeconds(10f);

            // Enable BulletSpawner1, Disable BulletSpawner
            bulletSpawner.SetActive(false);
            bulletSpawner1.SetActive(true);
            yield return new WaitForSeconds(10f);

            // Switch back to BulletSpawner
            bulletSpawner.SetActive(true);
            bulletSpawner1.SetActive(true);
            yield return new WaitForSeconds(10f);

        }
    }
}
