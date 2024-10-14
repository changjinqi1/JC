using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSelection : MonoBehaviour

{
    private GameObject childObject;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private bool isSelected = false;

    private NavIndicator navIndicator;

    void Start()
    {
        childObject = transform.GetChild(0).gameObject;

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        childObject.SetActive(false);

        navIndicator = GetComponent<NavIndicator>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    SelectObject();
                }

                else if (hit.collider.gameObject.CompareTag("Player") && isSelected)
                {
                    DeselectObject();
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            DeselectObject();
            childObject.SetActive(false);
        }
    }

    void SelectObject()
    {
        isSelected = true;
        childObject.SetActive(true);

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }
        if (navIndicator != null)
        {
            navIndicator.enabled = true;
        }
    }

    void DeselectObject()
    {
        isSelected = false;

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }
        if (navIndicator != null)
        {
            navIndicator.enabled = false;
        }
    }
}
