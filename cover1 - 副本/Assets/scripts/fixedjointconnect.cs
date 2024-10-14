using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixedjointconnect : MonoBehaviour
{
    private FixedJoint fixedJoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && fixedJoint != null)
        {
            Destroy(fixedJoint);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Treasure") && fixedJoint == null)
        
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = collision.rigidbody;
        }
    }