using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changeface : MonoBehaviour
{
    public float rotationAngle = 90f;    // The angle to rotate when 'D' is pressed
    public TilttheTable tiltScript;      // Reference to the TilttheTable script

    void Start()
    {
        // Ensure we have a reference to the TilttheTable script
        if (tiltScript == null)
        {
            tiltScript = GetComponent<TilttheTable>();
        }
    }

    void Update()
    {
        // Check if the 'D' key is pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Start the process of rotating and disabling the TilttheTable script for 1 second
            StartCoroutine(RotateAndUpdateTilttheTable());
        }
    }

    IEnumerator RotateAndUpdateTilttheTable()
    {
        // Disable the TilttheTable script to prevent conflicts
        if (tiltScript != null)
        {
            tiltScript.enabled = false;
        }

        // Rotate the parent and all children
        transform.Rotate(0, 0, rotationAngle);

        // Wait for 0.1 second to allow the rotation to settle
        yield return new WaitForSeconds(0.1f);

        // Update the initialRotation in TilttheTable to reflect the new state
        if (tiltScript != null)
        {
            tiltScript.UpdateInitialRotation(transform.rotation);
        }

        // Re-enable the TilttheTable script
        if (tiltScript != null)
        {
            tiltScript.enabled = true;
        }
    }
}
