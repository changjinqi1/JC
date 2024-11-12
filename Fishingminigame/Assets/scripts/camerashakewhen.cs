using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camerashakewhen : MonoBehaviour

{
    [Header("Slider Settings")]
    public Slider slider;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float shakeMagnitude = 0.1f;
    public float shakeFrequency = 0.05f;

    private Vector3 originalCameraPosition;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider is not assigned.");
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform is not assigned.");
            return;
        }

        originalCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        if (slider.value > 0.7 && slider.value < 0.98)
        {
            if (shakeCoroutine == null)
            {
                shakeCoroutine = StartCoroutine(ShakeCamera());
            }
        }
        else
        {
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                shakeCoroutine = null;
                cameraTransform.position = originalCameraPosition;
            }
        }
    }

    private IEnumerator ShakeCamera()
    {
        while (true)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.position = originalCameraPosition + new Vector3(offsetX, offsetY, 0);

            yield return new WaitForSeconds(shakeFrequency);
        }
    }
}
