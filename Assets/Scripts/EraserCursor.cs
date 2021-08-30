using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserCursor : MonoBehaviour
{
    
    void Update()
    {
        // Scale the eraser object to be the same size as the erasing radius
        float scale = GameObject.FindObjectOfType<LineEraser>().EraseRadius;
        this.transform.localScale = new Vector3(scale, scale, scale);
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        transform.position = mousePos;
    }
}
