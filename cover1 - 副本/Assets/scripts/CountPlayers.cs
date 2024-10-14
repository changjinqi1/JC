using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountPlayers : MonoBehaviour
{
    public int requiredPlayers = 3;
    private int playerCount = 0; 
    public TextMeshProUGUI playerCountText;
    public GameObject winText;
    public Vector3 zoneCenter = new Vector3(-1, 2.3f, 16); 
    public float zoneRadius = 5f;

    private GameObject[] players;

    void Start()
    {
        winText.SetActive(false);
        UpdatePlayerCountText();
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        playerCount = 0;

        foreach (GameObject player in players)
        {
            if (Vector3.Distance(player.transform.position, zoneCenter) <= zoneRadius)
            {
                playerCount++;
            }
        }

        UpdatePlayerCountText();

        if (playerCount >= requiredPlayers)
        {
            winText.SetActive(true);
        }
        else
        {
            winText.SetActive(false);
        }
    }

    void UpdatePlayerCountText()
    {
        playerCountText.text = $" {playerCount} / {requiredPlayers}";
    }
}
