using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameOverManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // Player name input
    public TextMeshProUGUI leaderboardText;
    public TextMeshProUGUI lastScoreText;

    private int lastScore;

    void Start()
    {
        // Retrieve the last score from PlayerPrefs
        lastScore = PlayerPrefs.GetInt("LastScore", 0);
        lastScoreText.text = " " + lastScore; // Show the player's last score
    }

    public void SubmitScore()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Anonymous"; // Default name if empty
        }

        // Save the score and update leaderboard
        SaveScore(playerName, lastScore);
        DisplayLeaderboard();
    }

    void SaveScore(string playerName, int score)
    {
        List<ScoreEntry> leaderboard = LoadLeaderboard();

        // Add the new score entry
        leaderboard.Add(new ScoreEntry(playerName, score));

        // Sort by highest score
        leaderboard = leaderboard.OrderByDescending(entry => entry.score).ToList();

        // Keep only the top x score
        if (leaderboard.Count > 5)
        {
            leaderboard.RemoveAt(leaderboard.Count - 1);
        }

        // Save updated leaderboard to PlayerPrefs
        string json = JsonUtility.ToJson(new Leaderboard(leaderboard));
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }

    void DisplayLeaderboard()
    {
        List<ScoreEntry> leaderboard = LoadLeaderboard();
        leaderboardText.text = "Leaderboard\n";

        int rank = 1;
        foreach (var entry in leaderboard)
        {
            leaderboardText.text += rank + ". " + entry.playerName + " - " + entry.score + "\n";
            rank++;
        }

        // Display the player's latest score below the leaderboard
        leaderboardText.text += "\nYour Score: " + lastScoreText.text;
    }

    List<ScoreEntry> LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString("Leaderboard", "{}");
        Leaderboard leaderboard = JsonUtility.FromJson<Leaderboard>(json);
        return leaderboard != null && leaderboard.entries != null ? leaderboard.entries : new List<ScoreEntry>();
    }

    [System.Serializable]
    public class Leaderboard
    {
        public List<ScoreEntry> entries;
        public Leaderboard(List<ScoreEntry> scores)
        {
            this.entries = scores;
        }
    }
}
