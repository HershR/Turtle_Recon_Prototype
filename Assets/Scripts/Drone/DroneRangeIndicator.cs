using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRangeIndicator : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public int subdivisions;
    public float radius;
    public Color startColor = Color.red;
    public Color endColor = Color.red;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = subdivisions;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        // Optional: Set a color gradient if you want
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        float angleStep = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions;
        for (int i = 0; i < subdivisions; i++)
        {
            float xPosition = radius * Mathf.Cos(i * angleStep);
            float yPosition = radius * Mathf.Sin(i * angleStep);
            Vector3 pointInCircle = new Vector3(xPosition, yPosition, -transform.position.z);
            lineRenderer.SetPosition(i, pointInCircle);
        }
    }
    public void Show()
    {
        lineRenderer.enabled = true;
    }
    public void Hide()
    {
        lineRenderer.enabled = false;
    }
    public void UpdateColor(Color newStartColor, Color newEndColor)
    {
        startColor = newStartColor;
        endColor = newEndColor;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        // Update the color gradient
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }
}
