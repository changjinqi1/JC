using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HintText : MonoBehaviour

{
    public GameObject hintText;
    private bool hasDisplayedHint = false;

    private void Start()
    {
        if (hintText != null)
        {
            hintText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasDisplayedHint)
        {
            ShowHint();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && hasDisplayedHint)
        {
            HideHint();
        }
    }

    private void ShowHint()
    {
        if (hintText != null)
        {
            hintText.SetActive(true);
            hasDisplayedHint = true;
        }
    }

    private void HideHint()
    {
        if (hintText != null)
        {
            hintText.SetActive(false);
        }
    }
}
