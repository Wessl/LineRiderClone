using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineData
{
    // index of line, each line maps index of point to world space position
    private Dictionary<int, float[]> lineRenderers = new Dictionary<int, float[]>();
    private string[] lineTypes;

    public LineData(Line[] lines)
    {
        int i = 0;
        lineTypes = new string[lines.Length];
        foreach (var line in lines)
        {
            // Convert points of type Vector2 from line class into float arrays
            Vector3[] points = line.PointsAsVec3Arr;
            float[] pointsAsFloats = new float[points.Length * 3];
            int y = 0;
            foreach (var point in points)
            {
                pointsAsFloats[y++] = point.x;
                pointsAsFloats[y++] = point.y;
                pointsAsFloats[y++] = point.z;
            }
            lineRenderers.Add(i, pointsAsFloats);
            lineTypes[i] = line.lineType;
            i++;
        }
    }

    public Dictionary<int, float[]> LineRenderers => lineRenderers;

    public string[] LineTypes => lineTypes;
}
