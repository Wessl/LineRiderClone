using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineEraser : MonoBehaviour
{
    // https://www.youtube.com/watch?v=dDj7DuHVV9E the vibe you must attain to use this code
    [SerializeField]
    private float eraseRadius;

    [SerializeField] private float eraserDistanceTolerance;
    public LayerMask lineMask;

    public GameObject lineBounce;
    public GameObject lineNormal;
    public GameObject lineBoost;

    private bool erasingActive;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;  // Ensure mouse isn't drawing when clicking UI buttons
        }

        if (!erasingActive)
        {
            return;
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Collider2D[] hits = Physics2D.OverlapCircleAll(worldMousePos, eraseRadius, lineMask);
            foreach (Collider2D hit in hits)
            {
                Debug.Log("yeah hello?");
                var pointsOnLine = hit.GetComponent<EdgeCollider2D>().points.ToList();
                // If there is just one point left on the line, you can safely remove the line's object
                if (pointsOnLine.Count <= 1)
                {
                    Destroy(hit.gameObject);
                    // We can also safely return immediately since nothing more needs to be done
                    return; 
                }
                // Get the point of the line that is closest to the 
                var closestPoint = hit.ClosestPoint(worldMousePos);
                int pointIndex = 0;
                int pointsCount = pointsOnLine.Count;
                if (Vector2.Distance(pointsOnLine[0], closestPoint) < eraserDistanceTolerance)
                {

                    RemovePointFromLine(hit, pointsOnLine[0]);
                    pointsOnLine.RemoveAt(0);
                    continue;
                }
                else if (Vector2.Distance(pointsOnLine[pointsCount - 1], closestPoint) < eraserDistanceTolerance)
                {

                    RemovePointFromLine(hit, pointsOnLine[pointsCount-1]);
                    pointsOnLine.RemoveAt(pointsCount - 1);
                    continue;
                }
                foreach (var point in pointsOnLine)
                {
                    if (Vector2.Distance(point, closestPoint) < eraserDistanceTolerance)
                    {
                        /*
                        // If the point that was collided with is the last point of a line, just remove it
                        if ( pointsOnLine[0].Equals(point) || pointsOnLine[pointsOnLine.Count-1].Equals(point))
                        {
                            Debug.Log("Removing endpoint");
                            pointsOnLine.Remove(point);
                            RemovePointFromLine(hit, point);
                        } */
                        
                        // Else you must split it into two new lists
                        pointsOnLine.Remove(point);
                        Debug.Log("Splitting into two new lines");
                        SplitIntoTwoNewLines(hit, pointsOnLine, pointIndex);
                        
                        // Only allow one point to be removed per frame? Idk
                        // rather than return I would prefer to jump back out to the next hit in the outermost for loop?
                        break;
                    }

                    pointIndex++;
                }
            }
        }
        
    }

    void RemovePointFromLine(Collider2D hit, Vector2 point)
    {
        hit.GetComponent<Line>().RemovePoint(point);
    }

    void NewLine(Collider2D hit, List<Vector2> pointsOnLine)
    {
        GameObject typeToInstantiate = GetLineType(hit);
        GameObject newLine = Instantiate(typeToInstantiate, new Vector2(0,0), Quaternion.identity);
        newLine.GetComponent<Line>().ConstructLineFromPoints(pointsOnLine.ToArray());
        Destroy(hit.gameObject);
    }

    void SplitIntoTwoNewLines(Collider2D hit, List<Vector2> pointsOnLine, int pointIndex)
    {
        Vector2[] pointArr1 = pointsOnLine.Take(pointIndex).ToArray();
        pointsOnLine.Reverse();
        Vector2[] pointArr2 = pointsOnLine.Take(pointsOnLine.Count - (pointIndex - 1)).ToArray();
        GameObject typeToInstantiate = GetLineType(hit);
        GameObject newLine1 = Instantiate(typeToInstantiate, new Vector2(0,0), Quaternion.identity);
        GameObject newLine2 = Instantiate(typeToInstantiate, new Vector2(0,0), Quaternion.identity);
        newLine1.GetComponent<Line>().ConstructLineFromPoints(pointArr1);
        newLine2.GetComponent<Line>().ConstructLineFromPoints(pointArr2);
        Destroy(hit.gameObject);
    }

    GameObject GetLineType(Collider2D hit)
    {
        GameObject typeToInstantiate;
        switch (hit.GetComponent<Line>().lineType)
        {
            case "bounce":
                typeToInstantiate = lineBounce;
                break;
            
            case "boost":
                typeToInstantiate = lineBoost;
                break;
            case "normal":
            default:
                typeToInstantiate = lineNormal;
                break;
        }

        return typeToInstantiate;
    }

    private void Start()
    {
    }


    public bool ErasingActive
    {
        get => erasingActive;
        set => erasingActive = value;
    }

    public float EraseRadius
    {
        get => eraseRadius;
        set => eraseRadius = value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), eraseRadius);
    }
}
