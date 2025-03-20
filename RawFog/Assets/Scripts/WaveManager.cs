using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public BulletWave wavespawner; // Assign BulletWave in the Inspector
    public float waveInterval = 10f; // Time between each wave

    void Start()
    {
        // Disable the wave spawner at the start
        wavespawner.gameObject.SetActive(false);

        // Start coroutine to enable the wave spawner after 10 seconds
        StartCoroutine(ActivateWaveSpawner());
    }

    IEnumerator ActivateWaveSpawner()
    {
        yield return new WaitForSeconds(10f); // Wait for 10 seconds
        wavespawner.gameObject.SetActive(true); // Enable the wave spawner
        wavespawner.SpawnWave(); // Spawn the first wave

        // Start the regular wave spawning loop
        StartCoroutine(TriggerWaveLoop());
    }

    IEnumerator TriggerWaveLoop()
    {
        while (true) // Infinite loop to keep triggering waves
        {
            yield return new WaitForSeconds(waveInterval); // Wait for the next wave interval
            wavespawner.SpawnWave();
        }
    }
}
