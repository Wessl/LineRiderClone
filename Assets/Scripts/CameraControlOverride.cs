using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControlOverride : MonoBehaviour
{
    public CinemachineVirtualCamera cmv_Camera;
    public Camera cam;
    private GameObject player;

    private Vector3 panOrigin;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            panOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
                            
            // Override the cinemachine setting temporarily
            cmv_Camera.Follow = null;
        }

        if (Input.GetMouseButton(2))
        {
            // Middle mouse controls the camera panning movement
            Vector3 difference = panOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            // Pan
            cmv_Camera.transform.position += difference;
        }

        // Scroll wheel zooming
        var scrollDelta = Input.mouseScrollDelta.y;
        cmv_Camera.m_Lens.OrthographicSize -= scrollDelta;
        if (cmv_Camera.m_Lens.OrthographicSize <= 1) cmv_Camera.m_Lens.OrthographicSize = 1;



    }

    public void CameraReset()
    {
        cmv_Camera.Follow = player.GetComponent<Transform>();
    }
}
