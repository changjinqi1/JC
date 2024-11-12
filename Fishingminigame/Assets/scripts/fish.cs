using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : MonoBehaviour
{
    private ScoreSystem scoreSystem;

    private void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        if (scoreSystem != null)
        {
            scoreSystem.SetLastFishType(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (scoreSystem != null)
        {
            scoreSystem.ClearLastFishType();
        }
    }
}