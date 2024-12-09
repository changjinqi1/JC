using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerstatecontroller : MonoBehaviour

{
    public GameObject object1; // First object to control
    public GameObject object2; // Second object to control

    public Color originalColor = Color.white; // Original color
    public Color stateAColor = Color.red;     // Color for state A

    private bool isStateA = false; // Tracks the current state
    private Renderer playerRenderer; // Renderer for the player to change colors
    private HingeJointControl object1Control;
    private HingeJointControl object2Control;

    private void Start()
    {
        // Get the renderer of the player object
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer == null)
        {
            Debug.LogError("No Renderer found on the player object.");
            return;
        }

        // Initialize colors and object rotation scripts
        playerRenderer.material.color = originalColor;

        if (object1 != null)
            object1Control = object1.GetComponent<HingeJointControl>();
        if (object2 != null)
            object2Control = object2.GetComponent<HingeJointControl>();

        EnableRotation(object1Control, true);
        EnableRotation(object2Control, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleState();
        }
    }

    private void ToggleState()
    {
        // Toggle between states
        isStateA = !isStateA;

        // Change the player's color based on the state
        playerRenderer.material.color = isStateA ? stateAColor : originalColor;

        // Enable rotation for the corresponding object
        EnableRotation(object1Control, !isStateA);
        EnableRotation(object2Control, isStateA);
    }

    private void EnableRotation(HingeJointControl rotationScript, bool enable)
    {
        if (rotationScript != null)
        {
            rotationScript.enabled = enable;
        }
    }
}
