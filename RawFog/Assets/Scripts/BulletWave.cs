using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWave : MonoBehaviour

{
    public GameObject bulletPrefab;
    public int bulletsPerWave = 5; // Number of bullets in a wave
    public Transform player;
    public float spawnMargin = 0.5f;
    public float minDistanceBetweenBullets = 1f;

    private float screenWidth, screenHeight;
    private List<Vector2> activeBulletPositions = new List<Vector2>();

    void Start()
    {
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize;
    }

    public void SpawnWave()
    {
        StartCoroutine(SpawnBulletsInWave());
    }

    IEnumerator SpawnBulletsInWave()
    {
        int bulletsSpawned = 0;
        while (bulletsSpawned < bulletsPerWave)
        {
            SpawnBullet();
            bulletsSpawned++;
            yield return new WaitForSeconds(0.2f); // Slight delay between each bullet in the wave
        }
    }

    void SpawnBullet()
    {
        if (player == null) return;

        Vector2 spawnPos;
        int maxAttempts = 10;

        do
        {
            spawnPos = GetRandomEdgePosition();
            maxAttempts--;
        }
        while (IsPositionOverlapping(spawnPos) && maxAttempts > 0);

        if (maxAttempts <= 0) return;

        activeBulletPositions.Add(spawnPos);

        Vector2 endPos = GetExitPointBeyondPlayer(spawnPos, player.position);
        GameObject newBullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        LineRenderer trajectoryRenderer = newBullet.GetComponent<LineRenderer>();
        if (trajectoryRenderer == null)
        {
            trajectoryRenderer = newBullet.AddComponent<LineRenderer>();
        }

        trajectoryRenderer.startWidth = 0.05f;
        trajectoryRenderer.endWidth = 0.05f;
        trajectoryRenderer.useWorldSpace = true;

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.trajectoryRenderer = trajectoryRenderer;
        bulletScript.SetTrajectory(spawnPos, player.position, endPos);

        Destroy(newBullet, 10f);
    }

    Vector2 GetRandomEdgePosition()
    {
        int edge = Random.Range(0, 4);
        float x = 0, y = 0;

        switch (edge)
        {
            case 0: x = -screenWidth - spawnMargin; y = Random.Range(-screenHeight, screenHeight); break;
            case 1: x = screenWidth + spawnMargin; y = Random.Range(-screenHeight, screenHeight); break;
            case 2: x = Random.Range(-screenWidth, screenWidth); y = screenHeight + spawnMargin; break;
            case 3: x = Random.Range(-screenWidth, screenWidth); y = -screenHeight - spawnMargin; break;
        }

        return new Vector2(x, y);
    }

    Vector2 GetExitPointBeyondPlayer(Vector2 startPos, Vector2 playerPos)
    {
        Vector2 directionToPlayer = (playerPos - startPos).normalized;
        float exitDistance = screenWidth + screenHeight;
        return playerPos + directionToPlayer * exitDistance;
    }

    bool IsPositionOverlapping(Vector2 position)
    {
        foreach (Vector2 existingPos in activeBulletPositions)
        {
            if (Vector2.Distance(existingPos, position) < minDistanceBetweenBullets)
                return true;
        }
        return false;
    }
}
