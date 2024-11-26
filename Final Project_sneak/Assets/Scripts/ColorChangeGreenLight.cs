using System.Collections;
using UnityEngine;

public class ColorChangeGreenLight : MonoBehaviour
{
    [Header("Color Settings")]
    public Color colorC = Color.green;
    public Color emissionColorC = Color.green;
    private Color originalColor;
    private Color originalEmissionColor;

    [Header("Timing Settings")]
    public float toColorDuration = 2f;
    public float toOriginalDuration = 0.5f;
    public float waitTime = 2f;
    public float initialWaitTime = 5f;

    private Renderer objectRenderer;
    private Material objectMaterial;
    private bool isColorC = false;

    public static bool IsInColorC { get; private set; }

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

        StartCoroutine(StartWithDelay());
    }

    IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(initialWaitTime);
        StartCoroutine(ColorChangeCycle());
    }

    IEnumerator ColorChangeCycle()
    {
        while (true)
        {
            yield return ChangeColor(originalColor, colorC, originalEmissionColor, emissionColorC, toColorDuration);
            isColorC = true;
            IsInColorC = true;

            yield return new WaitForSeconds(toColorDuration);
            isColorC = false;
            IsInColorC = false;

            yield return ChangeColor(colorC, originalColor, emissionColorC, originalEmissionColor, toOriginalDuration);

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
