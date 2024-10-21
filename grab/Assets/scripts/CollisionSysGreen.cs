using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSysGreen : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("gt"))
        {
            Destroy(collision.gameObject);
            scoreManager.AddPoints(1);
        }
        else if (collision.gameObject.CompareTag("gc"))
        {
            Destroy(collision.gameObject);
            scoreManager.AddPoints(3);
        }
        else if (collision.gameObject.CompareTag("rt"))
        {
            Destroy(collision.gameObject);
            scoreManager.SubtractPoints(1);
        }
        else if (collision.gameObject.CompareTag("rc"))
        {
            Destroy(collision.gameObject);
            scoreManager.SubtractPoints(1);
        }
        else if (collision.gameObject.CompareTag("trash"))
        {
            Destroy(collision.gameObject);
            scoreManager.SubtractPoints(3);
        }
    }
}
