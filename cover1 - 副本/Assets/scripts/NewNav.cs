using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNav : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    private bool isAttachedToTreasure = false;
    private bool isAgentEnabled = true;
    private Transform treasureTransform;

    void Update()
    {
        if (!isAttachedToTreasure || isAgentEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                Ray worldRay = Camera.main.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
                {
                    agent.SetDestination(hitInfo.point);
                }
            }
        }
        else if (isAttachedToTreasure)
        {
           
            FollowTreasure();
        }
    }

    public void AttachToTreasure(GameObject treasure)
    {
        if (treasure.CompareTag("Treasure"))
        {
            isAttachedToTreasure = true;
            treasureTransform = treasure.transform;
            DisableNavMeshAgent();
        }
    }

    private void DisableNavMeshAgent()
    {
        if (agent != null)
        {
            agent.enabled = false;
            isAgentEnabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (isAttachedToTreasure && !isAgentEnabled)
        {
            EnableNavMeshAgent();
        }
    }

    private void EnableNavMeshAgent()
    {
        if (agent != null)
        {
            agent.enabled = true;
            isAgentEnabled = true;
        }
    }

    public void DetachFromTreasure()
    {
        isAttachedToTreasure = false;

        if (agent != null)
        {
            agent.enabled = true;
            isAgentEnabled = true;
        }
    }

    private void FollowTreasure()
    {
        if (treasureTransform != null)
        {
            transform.position = treasureTransform.position;
            transform.rotation = treasureTransform.rotation;
        }
    }
}