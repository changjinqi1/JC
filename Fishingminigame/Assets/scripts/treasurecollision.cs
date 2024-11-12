using System.Collections;
using UnityEngine;

public class TreasureCollisionManager : MonoBehaviour
{
    public float fadeDuration = 3f;

    private GameObject treasureBox;
    private GameObject qteBar;
    private bool isFading = false;

    private void Update()
    {
        if (qteBar == null)
        {
            qteBar = GameObject.FindGameObjectWithTag("QTEbar");
        }
        if (treasureBox == null)
        {
            treasureBox = GameObject.FindGameObjectWithTag("treasure");
            isFading = false; // Reset fading if treasureBox is re-spawned
        }

        // Only proceed if both objects exist and we aren't already fading
        if (qteBar != null && treasureBox != null && !isFading)
        {
            if (IsCollidingWithTreasureBox())
            {
                StartCoroutine(FadeTreasureBoxToWhite());
                isFading = true; // Set fading to prevent re-triggering
            }
        }
    }

    private bool IsCollidingWithTreasureBox()
    {
        Collider2D qteCollider = qteBar.GetComponent<Collider2D>();
        Collider2D treasureCollider = treasureBox.GetComponent<Collider2D>();

        if (qteCollider != null && treasureCollider != null)
        {
            return qteCollider.IsTouching(treasureCollider);
        }
        return false;
    }

    private IEnumerator FadeTreasureBoxToWhite()
    {
        SpriteRenderer spriteRenderer = treasureBox.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            yield break;
        }

        Color initialColor = spriteRenderer.color;
        Color targetColor = Color.white;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        spriteRenderer.color = targetColor;

        Destroy(treasureBox);
        treasureBox = null;
        isFading = false;
    }
}
