using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orangecolorchange : MonoBehaviour

{
    [Header("Color Settings")]
    public Color colorO = new Color(1f, 0.5f, 0f); // Orange color
    public Color emissionColorO = new Color(1f, 0.5f, 0f); // Orange emission color
    private Color originalColor;
    private Color originalEmissionColor;

    [Header("Timing Settings")]
    public float toColorODuration = 2f;
    public float toOriginalDuration = 0.5f;
    public float waitTime = 2f;

    private Renderer objectRenderer;
    private Material objectMaterial;
    private bool isColorO = false;

    public static bool IsInColorO { get; private set; }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer attached to the object!");
            enabled = false;
            return;
        }

        objectMaterial = objectRenderer.material;
        originalColor = objectMaterial.color;

        if (objectMaterial.HasProperty("_EmissionColor"))
        {
            originalEmissionColor = objectMaterial.GetColor("_EmissionColor");
        }
        else
        {
            Debug.LogWarning("Material does not have an EmissionColor property. Ensure Emission is enabled in the shader.");
        }

        objectMaterial.EnableKeyword("_EMISSION");

        StartCoroutine(ColorChangeCycle());
    }

    IEnumerator ColorChangeCycle()
    {
        while (true)
        {
            yield return ChangeColor(originalColor, colorO, originalEmissionColor, emissionColorO, toColorODuration);
            isColorO = true;
            IsInColorO = true;

            yield return new WaitForSeconds(toColorODuration);
            isColorO = false;
            IsInColorO = false;

            yield return ChangeColor(colorO, originalColor, emissionColorO, originalEmissionColor, toOriginalDuration);

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator ChangeColor(Color fromColor, Color toColor, Color fromEmission, Color toEmission, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            objectMaterial.color = Color.Lerp(fromColor, toColor, t);

            if (objectMaterial.HasProperty("_EmissionColor"))
            {
                Color currentEmission = Color.Lerp(fromEmission, toEmission, t);
                objectMaterial.SetColor("_EmissionColor", currentEmission);
            }

            yield return null;
        }

        objectMaterial.color = toColor;

        if (objectMaterial.HasProperty("_EmissionColor"))
        {
            objectMaterial.SetColor("_EmissionColor", toEmission);
        }
    }
}
