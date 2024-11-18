using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintfollow : MonoBehaviour
{
    public Transform hooK;
    private Renderer objectARenderer;
    public float maxRadius = 1.5f;
    public float minRadius = 0.2f;
    public float maxHeight = 10.0f;

    void Start()
    {
        objectARenderer = GetComponent<Renderer>();
        if (objectARenderer == null)
        {
            Debug.LogError("Object A does not have a Renderer component.");
        }
    }

    void Update()
    {
        if (hooK != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = hooK.position.z;
            transform.position = newPosition;

            float normalizedHeight = Mathf.Clamp01(hooK.position.y / maxHeight);
            float newRadius = Mathf.Lerp(maxRadius, minRadius, normalizedHeight);

            transform.localScale = new Vector3(newRadius, newRadius, newRadius);
        }
    }
}
