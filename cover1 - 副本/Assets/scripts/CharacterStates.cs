using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Active,
        Carry,
        TryToCarry
    }

    public PlayerState currentState = PlayerState.Idle;
    public int requiredPlayersToCarry = 3;
    public bool isAttachedToTreasure = false;
    public UnityEngine.AI.NavMeshAgent agent;

    private static List<CharacterStates> attachedPlayers = new List<CharacterStates>();
    private Vector3 originalPosition;
    private GameObject currentTreasure;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        originalPosition = transform.position;
        SetState(PlayerState.Idle);
    }

    private void Update()
    {
        HandlePlayerSelection();
        HandleMovement();
        HandleTreasureDetach();
    }

    private void HandlePlayerSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    SetState(PlayerState.Active);
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (currentState == PlayerState.Active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                Ray worldRay = Camera.main.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.CompareTag("Ground"))
                    {
                        agent.SetDestination(hitInfo.point);
                    }
                }
            }
        }
        else if (currentState == PlayerState.Carry && currentTreasure != null)
        {

            transform.position = currentTreasure.transform.position;
            transform.rotation = currentTreasure.transform.rotation;
        }
    }

    private void HandleTreasureDetach()
    {
        if (currentState == PlayerState.Carry && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == currentTreasure.transform)
                {
                    DetachFromTreasure();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure") && currentState == PlayerState.Active)
        {
            if (!isAttachedToTreasure)
            {
                currentTreasure = other.gameObject;
                attachedPlayers.Add(this);
                isAttachedToTreasure = true;
                Debug.Log($"{gameObject.name} attached to the treasure {currentTreasure.name}.");

                if (attachedPlayers.Count >= requiredPlayersToCarry)
                {
                    foreach (var player in attachedPlayers)
                    {
                        player.agent.enabled = false;
                        player.SetState(PlayerState.Carry);
                    }
                }
                else
                {
                    SetState(PlayerState.TryToCarry);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentTreasure && isAttachedToTreasure)
        {
            DetachFromTreasure();
        }
    }

    private void DetachFromTreasure()
    {
        attachedPlayers.Remove(this);
        isAttachedToTreasure = false;
        currentTreasure = null;
        agent.enabled = true;
        transform.position = originalPosition;

        if (attachedPlayers.Count < requiredPlayersToCarry)
        {
            foreach (var player in attachedPlayers)
            {
                player.agent.enabled = true;
                player.SetState(PlayerState.Active);
            }
        }

        SetState(PlayerState.Active);
        Debug.Log($"{gameObject.name} has detached from the treasure.");
    }

    private void SetState(PlayerState newState)
    {
        currentState = newState;


        switch (newState)
        {
            case PlayerState.Idle:
                Debug.Log("Player is now in Idle state.");
                break;
            case PlayerState.Active:
                Debug.Log("Player is now in Active state.");
                break;
            case PlayerState.Carry:
                Debug.Log("Player is now carrying the treasure.");
                break;
            case PlayerState.TryToCarry:
                Debug.Log("Player is trying to carry the treasure but needs more players.");
                break;
        }

    }
}
