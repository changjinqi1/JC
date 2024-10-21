using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
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

    public void AddPoints(int points)
    {
        playerPoints += points;
        UpdatePointsText();
        CheckWinCondition();
        CheckLoseCondition();
    }

    public void SubtractPoints(int points)
    {
        playerPoints -= points;
        UpdatePointsText();
        CheckWinCondition();
        CheckLoseCondition();
    }

    void UpdatePointsText()
    {
        pointsText.text = "Points: " + playerPoints;
    }

    void CheckWinCondition()
    {
        if (playerPoints >= 15)
        {
            winText.text = "You Win!";
            loseText.text = "";
            Time.timeScale = 0f;
        }
    }

    void CheckLoseCondition()
    {
        if (playerPoints < -4)
        {
            loseText.text = "You Lose!";
            winText.text = "";
            Time.timeScale = 0f;
        }
    }
}
