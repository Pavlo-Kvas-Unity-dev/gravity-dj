using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent	(typeof(LineRenderer))]
public class Circle : MonoBehaviour
{
    public int vertexCount = 40;
    public float lineWidth = 0.2f;
    public float radius;
    public bool cirleFillscreen;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private bool drawGizmos;

    private void OnValidate()
    {
        SetWidth(lineWidth);
    }

    void Awake()
    {
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    public void Init(float curCircleRadius)
    {
        radius = curCircleRadius;
        
        SetWidth(lineWidth);

        if (cirleFillscreen)
        {
            radius = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMax, 0f)),
                         Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMin, 0f))) * .5f -
                     lineWidth;
        }

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0;

        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius*Mathf.Cos(theta), radius*Mathf.Sin(theta), 0f);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    private void SetWidth(float circleWidth)
    {
        if (lineRenderer == null) return;
        
        lineRenderer.widthMultiplier = circleWidth;
    }


    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldPos = Vector3.zero;
        for (int i = 0; i < vertexCount + 1; i++)
        {
            Vector3 pos = new Vector3	(radius	* Mathf.Cos	(theta), radius	* Mathf.Sin	(theta), 0f);
            Gizmos.DrawLine(oldPos, transform.position + pos);
            oldPos = transform.position + pos;
            theta += deltaTheta;
        }
    }

    public void SetColor(Color color)
    {
        lineRenderer.material.color = color;
        lineRenderer.endColor = color;
        lineRenderer.startColor = color;
    }
}
