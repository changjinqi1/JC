using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    private float currentHealth;
    public float healthRegenRate = 5f;
    private bool isInvulnerable = false;
    private bool isGameOver = false; // Prevent multiple GameOver calls

    private ScoringSystem scoringSystem; // Reference to the scoring system

    // Timer Variables
    public float gameTime = 40f; // 40-second countdown
    public TextMeshProUGUI timerText; // Assign in Inspector

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Get the ScoringSystem reference
        scoringSystem = FindObjectOfType<ScoringSystem>();

        StartCoroutine(RegenerateHealth());
        StartCoroutine(StartGameTimer()); // Start the countdown timer
    }

    // Countdown Timer Coroutine
    IEnumerator StartGameTimer()
    {
        while (gameTime > 0)
        {
            UpdateTimerUI();
            yield return new WaitForSeconds(1f);
            gameTime -= 1f;
        }

        // When time reaches 0, trigger Game Over
        GameOver();
    }

    // Update Timer UI
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(gameTime) + "s";

            // **Turn text red when 10 seconds left**
            if (gameTime <= 10)
            {
                timerText.color = Color.red;
            }
        }
    }

    // Detect when the player is hit by a bullet or collects a power-up
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvulnerable)
        {
            if (collision.CompareTag("Bullet")) // Bullet hits player
            {
                TakeDamage(30);
                StartCoroutine(InvulnerabilityCooldown());

                // Notify the scoring system that the player got hit
                if (scoringSystem != null)
                {
                    scoringSystem.OnPlayerHit();
                }

                // Destroy the bullet after hitting the player
                Destroy(collision.gameObject);
            }
            else if (collision.CompareTag("PowerUps")) // Player collects power-up
            {
                GetHealed(30);
                StartCoroutine(InvulnerabilityCooldown());

                Debug.Log("Collected Power-Up!");

                // Optional: Play power-up sound effect
                if (TryGetComponent<AudioSource>(out AudioSource audio))
                {
                    audio.Play();
                }

                // Add power-up to inventory
                InventorySystem inventory = FindObjectOfType<InventorySystem>();
                if (inventory != null)
                {
                    inventory.AddPowerUp();
                }

                // Destroy the power-up after collection
                Destroy(collision.gameObject);
            }
        }
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    public void StartInvulnerability()
    {
        StartCoroutine(InvulnerabilityCooldown());
    }

    public void TakeDamage(float damage)
    {
        if (isGameOver) return; // Prevent taking damage after Game Over

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;

        // Optional: Flash red when taking damage
        StartCoroutine(DamageFlashEffect());

        if (currentHealth <= 0 && !isGameOver)
        {
            GameOver();
        }
    }

    void GetHealed(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;
    }

    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            if (currentHealth > 0 && currentHealth < maxHealth)
            {
                currentHealth += healthRegenRate * Time.deltaTime;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                healthSlider.value = currentHealth;
            }
            yield return null;
        }
    }

    IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(0.5f);
        isInvulnerable = false;
    }

    public void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls
        isGameOver = true;

        Debug.Log("Game Over!");
        int finalScore = scoringSystem != null ? Mathf.FloorToInt(scoringSystem.GetFinalScore()) : 0;

        // Store the final score in PlayerPrefs for the GameOver scene
        PlayerPrefs.SetInt("LastScore", finalScore);
        PlayerPrefs.Save();

        // Load the GameOver scene
        SceneManager.LoadScene("GameOverScene");
    }

    IEnumerator DamageFlashEffect()
    {
        Image damageImage = healthSlider.GetComponentInChildren<Image>();
        if (damageImage != null)
        {
            Color originalColor = damageImage.color;
            damageImage.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            damageImage.color = originalColor;
        }
    }
}
