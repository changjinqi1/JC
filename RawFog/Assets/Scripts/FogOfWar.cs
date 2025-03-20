using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour

{
    private List<SpriteRenderer> bulletRenderers = new List<SpriteRenderer>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            SpriteRenderer bulletRenderer = collision.GetComponent<SpriteRenderer>();
            if (bulletRenderer != null)
            {
                StartCoroutine(FadeOut(bulletRenderer));
                bulletRenderers.Add(bulletRenderer);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            SpriteRenderer bulletRenderer = collision.GetComponent<SpriteRenderer>();
            if (bulletRenderer != null)
            {
                StartCoroutine(FadeIn(bulletRenderer));
                bulletRenderers.Remove(bulletRenderer);
            }
        }
    }

    IEnumerator FadeOut(SpriteRenderer spriteRenderer)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }

    IEnumerator FadeIn(SpriteRenderer spriteRenderer)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 1f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }
}
