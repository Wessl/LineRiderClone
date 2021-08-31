using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineCreator : MonoBehaviour
{
    private GameObject linePrefab;
    public GameObject normalLinePrefab;
    Line activeLine;
    public Camera mainCamera;
    
    private bool drawingActive;
    private bool straightLinesOnly;
    
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;  // Ensure mouse isn't drawing when clicking UI buttons
        }
        
        if (!drawingActive)
        {
            return;
        }

        if (straightLinesOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject lineGO = Instantiate(linePrefab);
                activeLine = lineGO.GetComponent<Line>();
            }
    
            if (Input.GetMouseButtonUp(0))
            {
                activeLine = null;
            }
    
            if (activeLine != null)
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                activeLine.UpdateLine(mousePos, true, true);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject lineGO = Instantiate(linePrefab);
                activeLine = lineGO.GetComponent<Line>();
            }
    
            if (Input.GetMouseButtonUp(0))
            {
                activeLine = null;
            }
    
            if (activeLine != null)
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                activeLine.UpdateLine(mousePos, true, false);
            }
        }
        
        
        
    }

    void Start()
    {
        linePrefab = normalLinePrefab;
        drawingActive = true;
        straightLinesOnly = false;
    }

    public void SetLinePrefab(GameObject linePrefabPicked)
    {
        this.linePrefab = linePrefabPicked;
    }

    public bool DrawingActive
    {
        get => drawingActive;
        set => drawingActive = value;
    }

    public bool StraightLinesOnly
    {
        get => straightLinesOnly;
        set => straightLinesOnly = value;
    }
}
