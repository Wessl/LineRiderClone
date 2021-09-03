using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCol;
    public float newPointThreshold;
    public String lineType;

    private List<Vector2> points;

    public void UpdateLine(Vector2 mousePos, bool shouldAverage, bool straightLine)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(mousePos, shouldAverage);
            return;
        }

        if (Vector2.Distance(points.Last(), mousePos) > newPointThreshold)
        {
            if (straightLine)
            {
                SetPointStraightLine(mousePos);
            }
            else
            {
                SetPoint(mousePos, shouldAverage);

            }
        }
    }

    private void SetPointStraightLine(Vector2 point)
    {
        if (points.Count >= 2)
        {
            // Remove all points between first and last
            points.RemoveAt(points.Count - 1);
        }

        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);

        if (points.Count > 1)
        {
            edgeCol.points = points.ToArray();
        }
    }

    void SetPoint(Vector2 point, bool shouldAverage)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
        
        // Average out the points
        if (points.Count > 2 && shouldAverage)
        {
            for (int i = Points.Count - 2; i > (points.Count - 1) - 15 && i > 1; i--)
            {
                var newPos = (lineRenderer.GetPosition(i + 1) + lineRenderer.GetPosition( i ) +
                              lineRenderer.GetPosition(i - 1)) / 3;
                lineRenderer.SetPosition(i, newPos);
                points[i] = newPos;
            }
        }

        if (points.Count > 1)
        {
            edgeCol.points = points.ToArray();
        }
    }

    public void RemovePoint(Vector2 point)
    {
        points.Remove(point);
        lineRenderer.SetPositions(points.ToArray().toVector3());
        lineRenderer.positionCount = points.Count;

        if (points.Count <= 1)
        {
            Destroy(gameObject);
        }
        else if (points.Count > 1)
        {
            edgeCol.points = points.ToArray();
        }
    }

    public string LineType => lineType;

    public void ConstructLineFromPoints(Vector2[] points)
    {
        if (points.Length <= 1)
        {
            return;
        }
        foreach (var point in points)
        {
            UpdateLine(point, false, false);
        }
    }

    public Vector3[] PointsAsVec3Arr => points.ToArray().toVector3();

    public List<Vector2> Points => points;
}

public static class MyVectorExtension
{
    public static Vector2[] toVector2 (this Vector3[] v3)
    {
        return System.Array.ConvertAll<Vector3, Vector2> (v3, getV2fromV3);
    }

    public static Vector3[] toVector3 (this Vector2[] v2)
    {
        return System.Array.ConvertAll<Vector2, Vector3> (v2, getV3fromV2);
    }
         
    public static Vector2 getV2fromV3 (Vector3 v3)
    {
        return new Vector2 (v3.x, v3.y);
    }
    
    public static Vector3 getV3fromV2 (Vector2 v2)
    {
        return new Vector3 (v2.x, v2.y, 0);
    }
}
