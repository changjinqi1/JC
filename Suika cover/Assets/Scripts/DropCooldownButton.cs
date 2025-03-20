using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropCooldownButton : MonoBehaviour
{
    public Button dropButton; // Reference to the UI Button
    public Image buttonImage; // Reference to the button's image (for color change)
    public Color normalColor = Color.white; // Default button color
    public Color pressedColor = Color.gray; // Button color when in cooldown
    public GameObject aimingLine; // Reference to the aiming line object

    private bool isCooldown = false;
    private float cooldownTime = 0.7f;

    void Start()
    {
        if (dropButton)
            dropButton.onClick.AddListener(OnDropButtonPressed);
    }

    void OnDropButtonPressed()
    {
        if (!isCooldown)
        {
            StartCoroutine(DropCooldown());
        }
    }

    private IEnumerator DropCooldown()
    {
        isCooldown = true;

        // Change button color to indicate cooldown
        if (buttonImage)
            buttonImage.color = pressedColor;

        // Hide aiming line
        if (aimingLine)
            aimingLine.SetActive(false);

        // Wait for cooldown
        yield return new WaitForSeconds(cooldownTime);

        // Reset button color
        if (buttonImage)
            buttonImage.color = normalColor;

        // Show aiming line again
        if (aimingLine)
            aimingLine.SetActive(true);

        isCooldown = false;
    }
}
