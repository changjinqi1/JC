using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;

    public ScoreEntry(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}

public class ScoringSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private float score = 0f;
    private float scoreIncreaseRate = 1f;
    private bool isDoubleScoreActive = false;
    private float timeSinceLastHit = 0f;

    void Start()
    {
        StartCoroutine(IncreaseScoreOverTime());
    }

    void Update()
    {
        timeSinceLastHit += Time.deltaTime;

        if (timeSinceLastHit >= 5f && !isDoubleScoreActive)
        {
            isDoubleScoreActive = true;
            scoreIncreaseRate *= 2f;
        }
    }

    IEnumerator IncreaseScoreOverTime()
    {
        while (true)
        {
            score += scoreIncreaseRate;
            UpdateScoreUI();
            yield return new WaitForSeconds(1f);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void OnPlayerHit()
    {
        timeSinceLastHit = 0f;
        if (isDoubleScoreActive)
        {
            isDoubleScoreActive = false;
            scoreIncreaseRate /= 2f;
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(score);
    }

    public float GetFinalScore()
    {
        return score;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        int finalScore = Mathf.FloorToInt(score);

        PlayerPrefs.SetInt("LastScore", finalScore);
        PlayerPrefs.Save();

        SceneManager.LoadScene("GameOverScene");
    }
}
