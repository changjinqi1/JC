using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarryMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private bool isAgentEnabled = false;
    private List<GameObject> attachedPlayers = new List<GameObject>();

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    void Update()
    {
        if (isAgentEnabled)
        {
            if (Input.GetMouseButtonDown(1))
            {
                DisableNavMeshAgent();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    navMeshAgent.SetDestination(hit.point);
                }
            }
        }
    }

    // Call this function to attach players to the treasure object
    public void AttachPlayer(GameObject player)
    {
        if (player.CompareTag("Player") && !attachedPlayers.Contains(player))
        {
            attachedPlayers.Add(player);

            // Enable the NavMeshAgent when 3 players are attached
            if (attachedPlayers.Count == 3)
            {
                EnableNavMeshAgent();
            }
        }
    }

    // Call this function to detach players from the treasure object
    public void DetachPlayer(GameObject player)
    {
        if (attachedPlayers.Contains(player))
        {
            attachedPlayers.Remove(player);

            // Disable the NavMeshAgent if less than 3 players remain
            if (attachedPlayers.Count < 3)
            {
                DisableNavMeshAgent();
            }
        }
    }

    // Enable the NavMeshAgent
    private void EnableNavMeshAgent()
    {
        navMeshAgent.enabled = true;
        isAgentEnabled = true;
    }

    // Disable the NavMeshAgent
    private void DisableNavMeshAgent()
    {
        navMeshAgent.enabled = false;
        isAgentEnabled = false;
    }

    void OnMouseDown()
    {
        // Left-click to enable the NavMeshAgent if it's not enabled
        if (Input.GetMouseButtonDown(0) && !isAgentEnabled && attachedPlayers.Count == 3)
        {
            EnableNavMeshAgent();
        }

        // Right-click to disable the NavMeshAgent if it's enabled
        if (Input.GetMouseButtonDown(1) && isAgentEnabled)
        {
            DisableNavMeshAgent();
        }
    }
}