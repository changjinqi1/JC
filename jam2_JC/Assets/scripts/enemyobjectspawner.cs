using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyobjectspawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public float spawnInterval = 10f;
    public TextMeshProUGUI countdownText;

    private Vector3 spawnAreaMin = new Vector3(-22, 70, -16);
    private Vector3 spawnAreaMax = new Vector3(6, 100, 17);

    private float spawnTimer = 0f;

    void Start()
    {
        countdownText.gameObject.SetActive(false);
        spawnTimer = spawnInterval;
    }

    void Update()
    {

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnObjects();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnObjects()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            GameObject spawnedObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity);

            spawnedObject.AddComponent<controller1>().Initialize(this);
        }
    }

    public void StartCountdown(GameObject obj)
    {
        StartCoroutine(CountdownAndDestroy(obj));
    }

    private System.Collections.IEnumerator CountdownAndDestroy(GameObject obj)
    {
        countdownText.gameObject.SetActive(true); 
        float countdown = 3f;

        while (countdown > 0)
        {
            countdownText.text = Mathf.Ceil(countdown).ToString();
            countdown -= Time.deltaTime;
            yield return null;
        }

        countdownText.gameObject.SetActive(false);
        Destroy(obj);
    }
}