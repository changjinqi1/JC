using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public Slider progressBar;
    public TextMeshProUGUI scoreText;

    public int scorePerNormal = 1;
    public int scorePerBlackFish = 3;
    public int scorePerRedFish = 2;
    private int currentScore = 0;

    private enum FishType { None, BlackFish, RedFish }
    private FishType lastFishType = FishType.None;  // Tracks the last fish type present in the scene

    private bool canScore = true;
    private float cooldownTime = 3f;

    private void Update()
    {
        if (progressBar.gameObject.activeSelf && progressBar.value > 0.95f && canScore)
        {
            AwardPoints();
            StartCoroutine(ScoreCooldown());
        }

        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void SetLastFishType(GameObject fish)
    {
        if (fish.name.Contains("BlackFish"))
        {
            lastFishType = FishType.BlackFish;
        }
        else if (fish.name.Contains("RedFish"))
        {
            lastFishType = FishType.RedFish;
        }
        else
        {
            lastFishType = FishType.None;
        }
    }

    public void ClearLastFishType()
    {
        lastFishType = FishType.None;
    }

    private void AwardPoints()
    {
        switch (lastFishType)
        {
            case FishType.BlackFish:
                currentScore += scorePerBlackFish;
                break;
            case FishType.RedFish:
                currentScore += scorePerRedFish;
                break;
            default:
                currentScore += scorePerNormal;
                break;
        }
    }

    private IEnumerator ScoreCooldown()
    {
        canScore = false;
        yield return new WaitForSeconds(cooldownTime);
        canScore = true;
    }
}
