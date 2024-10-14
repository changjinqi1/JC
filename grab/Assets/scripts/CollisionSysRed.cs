using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollisionSysRed : MonoBehaviour

{
    public int playerPoints = 0;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    void Start()
    {
        UpdatePointsText();
        winText.text = "";
        loseText.text = "";
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("rt"))
        {
            Destroy(collision.gameObject);
            playerPoints++;
            UpdatePointsText();
            CheckWinCondition();
        }
        else if (collision.gameObject.CompareTag("gt"))
        {
            Destroy(collision.gameObject);
            playerPoints--;
            UpdatePointsText();
            CheckLoseCondition();
        }
    }

    void UpdatePointsText()
    {
        pointsText.text = "Points: " + playerPoints;
    }

    void CheckWinCondition()
    {
        if (playerPoints > 4)
        {
            winText.text = "You Win!";
            loseText.text = "";
            Time.timeScale = 0f;
        }
    }

    void CheckLoseCondition()
    {
        if (playerPoints <= -4)
        {
            loseText.text = "You Lose!";
            winText.text = "";
            Time.timeScale = 0f;
        }
    }
}
