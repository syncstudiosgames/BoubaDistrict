using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDrawer : MonoBehaviour
{
    LineRenderer lineRenderer;

    public int subdivisions = 10;

    public void DrawCircle(float radius)
    {
        lineRenderer = GetComponent<LineRenderer>();

        if(lineRenderer == null)
        {
            Debug.LogWarning("No linerenderer assigned.");
            return;
        }

        float angleStep = 2f * Mathf.PI / subdivisions;

        lineRenderer.positionCount = subdivisions;

        for(int i = 0; i < subdivisions; i++)
        {
            float x = radius * Mathf.Cos(angleStep * i);
            float z = radius * Mathf.Sin(angleStep * i);

            Vector3 point = new Vector3(x, 0f, z);

            lineRenderer.SetPosition(i, point);
        }


    }
}
