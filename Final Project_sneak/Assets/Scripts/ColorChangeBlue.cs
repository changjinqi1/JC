using System.Collections;
using UnityEngine;

public class ColorChangeBlue : MonoBehaviour
{
    [Header("Color Settings")]
    public Color colorB = Color.blue;
    public Color emissionColorB = Color.blue;
    private Color originalColor;
    private Color originalEmissionColor;

    [Header("Timing Settings")]
    public float toColorBDuration = 2f;
    public float toOriginalDuration = 0.5f;
    public float waitTime = 2f;

    [Header("Lighting Settings")]
    public Light directionalLight; // Assign the directional light in the Inspector
    public float lightIntensityB = 6f; // Intensity when color is blue
    public float lightIntensityOriginal = 0f; // Intensity when color returns to original

    private Renderer objectRenderer;
    private Material objectMaterial;
    private bool isColorB = false;

    public static bool IsInColorB { get; private set; }

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

        if (directionalLight == null)
        {
            Debug.LogError("No directional light assigned!");
            enabled = false;
            return;
        }

        StartCoroutine(ColorChangeCycle());
    }

    IEnumerator ColorChangeCycle()
    {
        while (true)
        {
            // Change to Color B
            yield return ChangeColorAndLight(
                originalColor, colorB, originalEmissionColor, emissionColorB,
                toColorBDuration, lightIntensityOriginal, lightIntensityB
            );
            isColorB = true;
            IsInColorB = true;

            yield return new WaitForSeconds(toColorBDuration);
            isColorB = false;
            IsInColorB = false;

            // Change to the original color
            yield return ChangeColorAndLight(
                colorB, originalColor, emissionColorB, originalEmissionColor,
                toOriginalDuration, lightIntensityB, lightIntensityOriginal
            );

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator ChangeColorAndLight(
        Color fromColor, Color toColor, Color fromEmission, Color toEmission,
        float duration, float fromLightIntensity, float toLightIntensity)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Change material color
            objectMaterial.color = Color.Lerp(fromColor, toColor, t);

            // Change emission color
            if (objectMaterial.HasProperty("_EmissionColor"))
            {
                Color currentEmission = Color.Lerp(fromEmission, toEmission, t);
                objectMaterial.SetColor("_EmissionColor", currentEmission);
            }

            // Change light intensity
            directionalLight.intensity = Mathf.Lerp(fromLightIntensity, toLightIntensity, t);

            yield return null;
        }

        // Ensure final values are applied
        objectMaterial.color = toColor;

        if (objectMaterial.HasProperty("_EmissionColor"))
        {
            objectMaterial.SetColor("_EmissionColor", toEmission);
        }

        directionalLight.intensity = toLightIntensity;
    }
}
