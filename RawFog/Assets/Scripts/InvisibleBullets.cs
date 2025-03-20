using System.Collections;
using UnityEngine;

public class InvisibleBullets : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool inFog = false;
    public float checkRadius = 0.2f;
    private int fogLayer;
    private bool temporarilyRevealed = false;
    private Rigidbody2D rb;
    private Collider2D bulletCollider;
    private LineRenderer trajectoryRenderer;
    private bool hitPlayer = false; // Track if bullet hit the player
    private bool hitCR = false; // Track if bullet hit CR
    private ScoringSystem scoringSystem;
    public GameObject RedParticle;
    public GameObject YellowParticle;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();
        trajectoryRenderer = GetComponent<LineRenderer>();

        scoringSystem = FindObjectOfType<ScoringSystem>(); // Get scoring system reference
        fogLayer = 1 << LayerMask.NameToLayer("Fog");

        StartCoroutine(CheckFogStatus());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Bullet hit player - Applying damage!");

            HealthSystem playerHealth = collision.GetComponent<HealthSystem>();
            if (playerHealth != null && !playerHealth.IsInvulnerable())
            {
                playerHealth.TakeDamage(30);
                playerHealth.StartInvulnerability();
            }

            hitPlayer = true;

            // **Spawn RedParticle effect**
            if (RedParticle != null)
            {
                GameObject effect = Instantiate(RedParticle, transform.position, Quaternion.identity);
                Destroy(effect, 0.6f); // Destroy particle effect after 0.6s
            }

            // If the bullet hit CR first, award +4 score
            if (hitCR && scoringSystem != null)
            {
                scoringSystem.AddScore(4);
                Debug.Log("Bullet hit CR and then Player! +4 Score");
            }

            StartCoroutine(HitPlayerEffect());
        }
        else if (collision.CompareTag("CR"))
        {
            hitCR = true;
            StartCoroutine(CRScoreCountdown());
        }
    }

    IEnumerator CRScoreCountdown()
    {
        yield return new WaitForSeconds(0.5f);

        if (!hitPlayer) // Only award +8 if player didn't get hit in time
        {
            TemporarilyReveal();
            spriteRenderer.color = Color.yellow;
            Debug.Log("Bullet hit CR but no player within 0.5s! +8 Score");

            // **Spawn YellowParticle effect**
            if (YellowParticle != null)
            {
                GameObject effect = Instantiate(YellowParticle, transform.position, Quaternion.identity);
                Destroy(effect, 0.6f); // Destroy particle effect after 0.6s
            }

            if (scoringSystem != null)
            {
                scoringSystem.AddScore(8);
            }
        }
    }

    IEnumerator CheckFogStatus()
    {
        while (true)
        {
            if (!temporarilyRevealed) // Only check fog when not revealed
            {
                bool currentlyInFog = Physics2D.OverlapCircle(transform.position, checkRadius, fogLayer);

                if (currentlyInFog && !inFog)
                {
                    inFog = true;
                    StartCoroutine(FadeOut());
                }
                else if (!currentlyInFog && inFog)
                {
                    inFog = false;
                    StartCoroutine(FadeIn());
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOut()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;
        while (elapsed < duration && inFog && !temporarilyRevealed)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;
        while (elapsed < duration && (!inFog || temporarilyRevealed))
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 1f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }

    public void TemporarilyReveal()
    {
        Debug.Log("Bullet temporarily revealed!");

        temporarilyRevealed = true;
        StopAllCoroutines(); // Stop fog-checking coroutines

        // Make sure the bullet is fully visible
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        // Disable fog interactions temporarily
        inFog = false;
    }

    public void ResetVisibility()
    {
        StartCoroutine(FadeBackIntoFog());
    }

    IEnumerator FadeBackIntoFog()
    {
        yield return new WaitForSeconds(0.5f); // Wait before fading back
        inFog = true; // Allow fog to affect the bullet again
        StartCoroutine(CheckFogStatus()); // Restart fog-checking system
    }

    IEnumerator HitPlayerEffect()
    {
        TemporarilyReveal();
        spriteRenderer.color = Color.red; // Turn red on impact

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if (bulletCollider != null)
        {
            bulletCollider.enabled = false;
        }

        if (trajectoryRenderer != null)
        {
            trajectoryRenderer.startColor = Color.red;
            trajectoryRenderer.endColor = Color.red;
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
