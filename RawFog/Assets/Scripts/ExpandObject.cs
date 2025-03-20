using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandObject : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ExpandOverTime());
    }

    IEnumerator ExpandOverTime()
    {
        yield return StartCoroutine(ScaleObject(new Vector2(10f, 10f), new Vector2(6f, 6f), 5f));
        yield return StartCoroutine(ScaleObject(new Vector2(11f, 11f), new Vector2(6f, 6f), 5f));
        yield return StartCoroutine(ScaleObject(new Vector2(12f, 12f), new Vector2(8f, 8f), 10f));
        yield return StartCoroutine(ScaleObject(new Vector2(13f, 13f), new Vector2(8f, 8f), 5f));
    }

    IEnumerator ScaleObject(Vector2 startSize, Vector2 targetSize, float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float progress = timeElapsed / duration;
            transform.localScale = Vector3.Lerp(startSize, targetSize, progress);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetSize; // Ensure final size is set
    }
}
