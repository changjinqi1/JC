using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gouzhuayice : MonoBehaviour
{
    public HingeJoint hingeJoint;
    public float motorForce = 100f;
    public float returnSpeed = 100f;

    private JointMotor motor;

    void Start()
    {
        if (hingeJoint == null)
        {
            hingeJoint = GetComponent<HingeJoint>();
        }

        motor = hingeJoint.motor;
        motor.force = motorForce;
        hingeJoint.useMotor = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
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
        motor.targetVelocity = motorForce;
        hingeJoint.motor = motor;
    }

    private void ReturnToOriginalPosition()
    {
        float currentAngle = hingeJoint.angle;

        if (currentAngle > 0)
        {
            motor.targetVelocity = -returnSpeed;
        }
        else
        {
            motor.targetVelocity = 0;
        }

        hingeJoint.motor = motor;
    }
}
