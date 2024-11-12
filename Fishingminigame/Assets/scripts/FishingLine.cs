using UnityEngine;

public class FishingLine : MonoBehaviour
{
    public Transform player;
    private GameObject fishingFloat;
    private LineRenderer lineRenderer;

    [Header("Fishing Line Settings")]
    public string fishingFloatTag = "FishingFloat";
    public int lineResolution = 20;
    public float curveHeight = 2f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = lineResolution;

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (fishingFloat == null)
        {
            fishingFloat = GameObject.FindGameObjectWithTag(fishingFloatTag);
            if (fishingFloat != null)
            {
                lineRenderer.enabled = true;
            }
        }

        if (fishingFloat != null)
        {
            DrawCurvedLine();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void DrawCurvedLine()
    {
        Vector3 startPoint = player.position;
        Vector3 endPoint = fishingFloat.transform.position;

        for (int i = 0; i < lineResolution; i++)
        {
            float t = i / (float)(lineResolution - 1);

            Vector3 point = Vector3.Lerp(startPoint, endPoint, t);

            point.y += Mathf.Sin(t * Mathf.PI) * curveHeight;

            lineRenderer.SetPosition(i, point);
        }
    }
}
