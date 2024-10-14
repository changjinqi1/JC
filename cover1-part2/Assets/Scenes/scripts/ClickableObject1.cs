using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public Renderer myRenderer;

    Color originalColor;
    public Color highlight;

    bool isHighlighted;

    private void Start()
    {
        originalColor = myRenderer.material.color;
    }
    public void Hover()
    {
        myRenderer.material.color = highlight;
        isHighlighted = true;
    }

    private void Update()
    {
        if (isHighlighted)
        {
            isHighlighted = false;
            myRenderer.material.color = originalColor;
        }
    }
}
