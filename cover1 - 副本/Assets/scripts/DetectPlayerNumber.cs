using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerNumber : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    private List<GameObject> attachedPlayers = new List<GameObject>();
    private GameObject childObject;
    private bool isSelected = false;
    private NavIndicator navIndicator;

    void Start()
    {
        agent.enabled = false;

        childObject = transform.GetChild(0).gameObject;

        childObject.SetActive(false);

        navIndicator = GetComponent<NavIndicator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && attachedPlayers.Count == 1)
        {
            EnableNavMeshAgent();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    SelectObject();
                }
            }

            if (Input.GetMouseButtonDown(1) && isSelected)
            {
                DeselectObject();
                childObject.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(1) && agent.enabled)
        {
            DisableNavMeshAgent();
        }
    }

    void SelectObject()
    {
        if (attachedPlayers.Count == 1)
        {
            isSelected = true;
            childObject.SetActive(true);

            if (agent != null)
            {
                agent.enabled = true;
            }
            if (navIndicator != null)
            {
                navIndicator.enabled = true;
            }
        }
    }

    void DeselectObject()
    {
        isSelected = false;

        if (agent != null)
        {
            agent.enabled = false;
        }
        if (navIndicator != null)
        {
            navIndicator.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !attachedPlayers.Contains(other.gameObject))
        {
            attachedPlayers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && attachedPlayers.Contains(other.gameObject))
        {
            attachedPlayers.Remove(other.gameObject);
        }
    }

    private void EnableNavMeshAgent()
    {
        if (agent != null)
        {
            agent.enabled = true;
            Debug.Log("NavMeshAgent enabled.");
        }
    }

    private void DisableNavMeshAgent()
    {
        if (agent != null)
        {
            agent.enabled = false;
            Debug.Log("NavMeshAgent disabled.");
        }
    }
}