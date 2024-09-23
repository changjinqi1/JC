using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{
    public float shrinkDuration = 2f; 
    public float minScale = 0.01f;
    public float delayBeforeRestart = 3f;

    private bool isShrinking = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShrinking)
        {
            StartCoroutine(ShrinkAndDestroy());
        }
    }

    IEnumerator ShrinkAndDestroy()
    {
        isShrinking = true;

        Vector3 originalScale = transform.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;

            float scaleFactor = Mathf.Lerp(1f, minScale, elapsedTime / shrinkDuration);
            transform.localScale = originalScale * scaleFactor;

            yield return null;
        }

        Destroy(gameObject);

    }
}