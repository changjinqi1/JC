using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gouzhuayice : MonoBehaviour
{
    public HingeJoint hingeJoint;              // Reference to the HingeJoint component
    public float motorForce = 100f;            // Force to apply to bend the joint
    public float returnSpeed = 100f;            // Speed at which the joint returns to the original position

    private JointMotor motor;                   // Motor used by the hinge joint

    void Start()
    {
        // Ensure the HingeJoint component is assigned
        if (hingeJoint == null)
        {
            hingeJoint = GetComponent<HingeJoint>();
        }

        // Initialize the motor settings
        motor = hingeJoint.motor;
        motor.force = motorForce;
        hingeJoint.useMotor = true;              // Enable the motor
    }

    void Update()
    {
        // Check for space bar press
        if (Input.GetKey(KeyCode.Space))
        {
            BendJoint();
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    private void BendJoint()
    {
        // Set the motor to apply continuous force to bend the joint
        motor.targetVelocity = motorForce;  // Positive to bend the joint
        hingeJoint.motor = motor;
    }

    private void ReturnToOriginalPosition()
    {
        // Gradually reduce the angle towards the original position
        float currentAngle = hingeJoint.angle;

        // If the current angle is greater than zero, apply force to return
        if (currentAngle > 0)
        {
            motor.targetVelocity = -returnSpeed; // Negative to return to original position
        }
        else
        {
            motor.targetVelocity = 0; // Stop motor when close enough to original position
        }

        hingeJoint.motor = motor;
    }
}