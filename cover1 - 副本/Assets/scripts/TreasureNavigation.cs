using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreasureNavigation : MonoBehaviour
{
    private NavMeshAgent treasureAgent;

    private void Start()
    {
        treasureAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateTreasureAreaMask();
    }

    private void UpdateTreasureAreaMask()
    {
        List<CharacterStates> attachedPlayers = new List<CharacterStates>(FindObjectsOfType<CharacterStates>());
        bool isAnyPlayerAttached = false;

        int combinedAreaMask = ~0;

        foreach (var player in attachedPlayers)
        {
            if (player.isAttachedToTreasure)
            {
                NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();

                if (playerAgent != null)
                {
                    combinedAreaMask &= playerAgent.areaMask;
                    isAnyPlayerAttached = true;
                }
                else
                {
                    Debug.LogWarning($"Player {player.gameObject.name} does not have a NavMeshAgent!");
                }
            }
        }

        if (isAnyPlayerAttached)
        {
            treasureAgent.areaMask = combinedAreaMask;
            treasureAgent.enabled = true;
            Debug.Log($"Treasure can navigate on area mask: {treasureAgent.areaMask}");
        }
        else
        {
            treasureAgent.enabled = false;
            Debug.Log("No players attached to the treasure, disabling treasure navigation.");
        }
    }
}
