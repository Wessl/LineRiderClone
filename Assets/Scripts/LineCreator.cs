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
    private Player playerRef;
    
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

       
        if (Input.GetMouseButtonDown(0))
        {
            GameObject lineGO = Instantiate(linePrefab);
            activeLine = lineGO.GetComponent<Line>();
        }

        if (activeLine != null)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePos, true, straightLinesOnly);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (activeLine.Points.Count <= 1)
            {
                Destroy(activeLine.gameObject);
            }
            else
            {
                activeLine = null;
            }
        }
    }

    void Start()
    {
        linePrefab = normalLinePrefab;
        drawingActive = true;
        straightLinesOnly = false;
        playerRef = GameObject.FindWithTag("Player").GetComponent<Player>();
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
