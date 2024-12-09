using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerstatescriptlv5 : MonoBehaviour
{
    public GameObject[] originalStateObjects;
    public GameObject[] stateAObjects;

    public Color originalColor = Color.white;
    public Color stateAColor = Color.red;

    private bool isStateA = false;
    private Renderer playerRenderer;
    private List<HingeJointControl> originalStateControls = new List<HingeJointControl>();
    private List<HingeJointControl> stateAControls = new List<HingeJointControl>();

    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer == null)
        {
            Debug.LogError("No Renderer found on the player object.");
            return;
        }

        playerRenderer.material.color = originalColor;

        foreach (GameObject obj in originalStateObjects)
        {
            if (obj != null)
            {
                HingeJointControl control = obj.GetComponent<HingeJointControl>();
                if (control != null)
                {
                    originalStateControls.Add(control);
                    control.enabled = true;
                }
            }
        }

        foreach (GameObject obj in stateAObjects)
        {
            if (obj != null)
            {
                HingeJointControl control = obj.GetComponent<HingeJointControl>();
                if (control != null)
                {
                    stateAControls.Add(control);
                    control.enabled = false;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleState();
        }
    }

    private void ToggleState()
    {
        isStateA = !isStateA;

        playerRenderer.material.color = isStateA ? stateAColor : originalColor;

        EnableRotationForState(originalStateControls, !isStateA);
        EnableRotationForState(stateAControls, isStateA);
    }

    private void EnableRotationForState(List<HingeJointControl> controls, bool enable)
    {
        foreach (HingeJointControl control in controls)
        {
            if (control != null)
            {
                control.enabled = enable;
            }
        }
    }
}
