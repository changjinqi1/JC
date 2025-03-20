using UnityEngine;
using TMPro;
public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText; // Assign in Inspector

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // Load the saved score
        finalScoreText.text = "Final Score: " + finalScore;
    }
}
