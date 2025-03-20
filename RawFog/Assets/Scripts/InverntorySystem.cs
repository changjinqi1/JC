using System.Collections;
using UnityEngine;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public TextMeshProUGUI powerUpText;
    private int powerUpCount = 0;
    private bool canUsePowerUp = true;
    private PowerUpEffect powerUpEffect;

    // **Fog GameObject (Assign in Inspector)**
    public GameObject fogObject;
    private Renderer fogRenderer;
    private Color originalFogColor;

    // **Fog Particle System (Assign in Inspector)**
    public ParticleSystem fogParticle;

    void Start()
    {
        powerUpEffect = FindObjectOfType<PowerUpEffect>(); // Find PowerUpEffect script in the scene
        UpdatePowerUpUI();

        // Get the Renderer from the Fog GameObject
        if (fogObject != null)
        {
            fogRenderer = fogObject.GetComponent<Renderer>();
            if (fogRenderer != null)
            {
                originalFogColor = fogRenderer.material.color; // Store original color
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && powerUpCount > 0)
        {
            UsePowerUp();
        }
    }

    public void AddPowerUp()
    {
        powerUpCount++;
        UpdatePowerUpUI();

        if (powerUpCount > 1)
        {
            canUsePowerUp = false;
        }
    }

    private void UsePowerUp()
    {
        if (canUsePowerUp)
        {
            Debug.Log("Power-up used! Revealing invisible bullets and turning fog green.");

            powerUpEffect.ActivatePowerUp(5f); // Call the script to handle effect

            // **Turn off the Fog Particle System for 5 seconds**
            if (fogParticle != null)
            {
                fogParticle.Stop();
            }

            powerUpCount--;
            UpdatePowerUpUI();
            StartCoroutine(ResetFogEffect());

            if (powerUpCount <= 1)
            {
                canUsePowerUp = true;
            }
        }
        else
        {
            Debug.Log("Power-up function is disabled.");
        }
    }

    private IEnumerator ResetFogEffect()
    {
        yield return new WaitForSeconds(5f); // Wait 5 seconds

        // **Reset the fog color**
        if (fogRenderer != null)
        {
            fogRenderer.material.color = originalFogColor;
        }

        // **Turn the Fog Particle System back on**
        if (fogParticle != null)
        {
            fogParticle.Play();
        }
    }

    private void UpdatePowerUpUI()
    {
        powerUpText.text = "Eye: " + powerUpCount;
    }
}
