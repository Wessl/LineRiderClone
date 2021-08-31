using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineData
{
    // index of line, each line maps index of point to world space position
    private Dictionary<int, double[]> lineRenderers = new Dictionary<int, double[]>();
    private string[] lineTypes;

    public LineData(Line[] lines)
    {
        int i = 0;
        lineTypes = new string[lines.Length];
        foreach (var line in lines)
        {
            // Convert points of type Vector2 from line class into float arrays
            Vector3[] points = line.PointsAsVec3Arr;
            double[] pointsAsDoubles = new Double[points.Length * 3];
            int y = 0;
            foreach (var point in points)
            {
                pointsAsDoubles[y++] = point.x;
                pointsAsDoubles[y++] = point.y;
                pointsAsDoubles[y++] = point.z;
            }
            lineRenderers.Add(i, pointsAsDoubles);
            lineTypes[i] = line.lineType;
            i++;
        }
    }

    public Dictionary<int, double[]> LineRenderers => lineRenderers;

    public string[] LineTypes => lineTypes;
}
