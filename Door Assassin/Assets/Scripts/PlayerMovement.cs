using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;

    private NavMeshAgent agent;
    private Vector3 moveDirection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false; // We control rotation manually
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Get input direction relative to camera
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            // Convert local input direction to world space based on camera
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Remove any vertical component (keep only XZ plane)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Final movement direction
            moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

            Vector3 targetPosition = transform.position + moveDirection;
            agent.SetDestination(targetPosition);

            // Rotate player toward movement direction
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
        else
        {
            agent.ResetPath();
        }
    }
}
