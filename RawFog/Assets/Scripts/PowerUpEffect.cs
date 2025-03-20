using System.Collections;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
    private bool isPowerUpActive = false;

    public void ActivatePowerUp(float duration)
    {
        if (!isPowerUpActive)
        {
            StartCoroutine(RevealInvisibleBullets(duration));
        }
    }

    IEnumerator RevealInvisibleBullets(float duration)
    {
        isPowerUpActive = true;

        // Find all active invisible bullets
        InvisibleBullets[] bullets = FindObjectsOfType<InvisibleBullets>();
        foreach (InvisibleBullets bullet in bullets)
        {
            if (bullet != null)
            {
                bullet.TemporarilyReveal();
            }
        }

        yield return new WaitForSeconds(duration);

        // Find remaining bullets and reset their visibility
        bullets = FindObjectsOfType<InvisibleBullets>();
        foreach (InvisibleBullets bullet in bullets)
        {
            if (bullet != null)
            {
                bullet.ResetVisibility();
            }
        }

        isPowerUpActive = false;
    }
}
