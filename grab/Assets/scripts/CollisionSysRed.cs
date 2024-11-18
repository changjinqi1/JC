using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSysRed : MonoBehaviour
{
    private ScoreManager scoreManager;

    public ParticleSystem redSmoke;
    public ParticleSystem redFlame;

    private bool isCollisionDisabled = false;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCollisionDisabled) return;

        if (collision.gameObject.CompareTag("gt"))
        {
            Destroy(collision.gameObject);
            scoreManager.SubtractPoints(1);
        }
        else if (collision.gameObject.CompareTag("gc"))
        {
            Destroy(collision.gameObject);
            scoreManager.SubtractPoints(3);
        }
        else if (collision.gameObject.CompareTag("rt"))
        {
            Destroy(collision.gameObject);
            scoreManager.AddPoints(1);
            PlayParticleEffectAtCollision(collision, redSmoke);
        }
        else if (collision.gameObject.CompareTag("rc"))
        {
            Destroy(collision.gameObject);
            scoreManager.AddPoints(2);
            PlayParticleEffectAtCollision(collision, redSmoke);
        }
        else if (collision.gameObject.CompareTag("trash"))
        {
            Destroy(collision.gameObject);
            scoreManager.SubtractPoints(3);
            StopRedFlameTemporarily();
        }
    }

    private void PlayParticleEffectAtCollision(Collision collision, ParticleSystem particleSystem)
    {
        if (particleSystem != null)
        {
            particleSystem.transform.position = collision.GetContact(0).point;
            particleSystem.Play();
        }
        else
        {
            Debug.LogWarning("Particle system not assigned.");
        }
    }

    private void StopRedFlameTemporarily()
    {
        if (redFlame != null)
        {
            redFlame.Stop();
        }

        StartCoroutine(DisableCollisionTemporarily(5f));
    }

    private IEnumerator DisableCollisionTemporarily(float duration)
    {
        isCollisionDisabled = true;
        yield return new WaitForSeconds(duration);
        isCollisionDisabled = false;

        if (redFlame != null)
        {
            redFlame.Play();
        }
    }
}
