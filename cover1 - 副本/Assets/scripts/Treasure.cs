using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Navigate navigateScript;
    private int playerCount = 0;

    public ParticleSystem waterdropEffect; 
    public ParticleSystem happyEffect; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;
            CheckPlayerCount();
            waterdropEffect.gameObject.SetActive(true);
            waterdropEffect.Play();
            StartCoroutine(StopEffectAfterDuration(waterdropEffect, 3f)); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount--;
            CheckPlayerCount();
        }
    }

    private void CheckPlayerCount()
    {
        if (playerCount >= 3)
        {
            if (navigateScript != null)
            {
                navigateScript.enabled = true;
                happyEffect.gameObject.SetActive(true);
                happyEffect.Play();
                StartCoroutine(StopEffectAfterDuration(happyEffect, 3f));
            }
        }
        else
        {
            if (navigateScript != null)
            {
                navigateScript.enabled = false;
            }
        }
    }

    private IEnumerator StopEffectAfterDuration(ParticleSystem effect, float duration)
    {
        yield return new WaitForSeconds(duration);
        effect.Stop();
    }
}
