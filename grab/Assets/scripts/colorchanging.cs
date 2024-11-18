using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorchanging : MonoBehaviour

{
    public Color targetColor = Color.red;
    public float duration = 0.6f;

    private Renderer objectRenderer;
    private Color originalColor;
    private float timer;
    private bool isReverting;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null && objectRenderer.material.HasProperty("_Color"))
        {
            originalColor = objectRenderer.material.color;
        }
        else
        {
            Debug.LogError("The object does not have a valid material with a _Color property.");
            enabled = false;
        }
    }

    void Update()
    {
        if (objectRenderer == null) return;

        timer += Time.deltaTime;

        if (!isReverting)
        {
            float lerpFactor = timer / duration;
            objectRenderer.material.color = Color.Lerp(originalColor, targetColor, lerpFactor);

            if (lerpFactor >= 1f)
            {
                isReverting = true;
                timer = 0f;
            }
        }
        else
        {
            float lerpFactor = timer / duration;
            objectRenderer.material.color = Color.Lerp(targetColor, originalColor, lerpFactor);

            if (lerpFactor >= 1f)
            {
                isReverting = false;
                timer = 0f;
            }
        }
    }
}
