using UnityEngine;
using UnityEngine.UI;

public class OneTimeButton : MonoBehaviour
{
    private Button button; // Reference to the button

    void Start()
    {
        button = GetComponent<Button>(); // Get button component
        if (button != null)
        {
            button.onClick.AddListener(DisableButton);
        }
    }

    void DisableButton()
    {
        if (button != null)
        {
            button.interactable = false; // Disable button after first click
        }
    }
}
