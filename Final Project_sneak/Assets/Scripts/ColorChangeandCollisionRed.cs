using System.Collections;
using UnityEngine;

public class ColorChangeandCollisionRed : MonoBehaviour
{
    [Header("Color Settings")]
    public Color colorA = Color.red;
    public Color emissionColorA = Color.red;
    private Color originalColor;
    private Color originalEmissionColor;

    [Header("Timing Settings")]
    public float toColorADuration = 2f;
    public float toOriginalDuration = 0.5f;
    public float waitTime = 2f;

    private Renderer objectRenderer;
    private Material objectMaterial;
    private bool isColorA = false;

    public static bool IsInColorA { get; private set; }

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
            // Change to Color A
            yield return ChangeColor(originalColor, colorA, originalEmissionColor, emissionColorA, toColorADuration);
            isColorA = true;
            IsInColorA = true;

            yield return new WaitForSeconds(toColorADuration);
            isColorA = false;
            IsInColorA = false;

            // Change to the original color
            yield return ChangeColor(colorA, originalColor, emissionColorA, originalEmissionColor, toOriginalDuration);

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

        //check final color
        objectMaterial.color = toColor;

        if (objectMaterial.HasProperty("_EmissionColor"))
        {
            objectMaterial.SetColor("_EmissionColor", toEmission);
        }
    }
}
