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
    static float t = 0;
    private float orthogCameraSize;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        orthogCameraSize = cmv_Camera.m_Lens.OrthographicSize;
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

        // Scroll wheel zooming with some lerping for smoothness
        float scrollDelta = Input.mouseScrollDelta.y;
        if (!scrollDelta.Equals(0f))
        {
            // Apply scroll delta, with less effect when near closest possible point to enable greater control in high zoom
            orthogCameraSize -= Input.mouseScrollDelta.y * orthogCameraSize/(4);
            // Start timer
            t = 0 + Time.deltaTime;
        }

        cmv_Camera.m_Lens.OrthographicSize = Mathf.Lerp(cmv_Camera.m_Lens.OrthographicSize, orthogCameraSize, t); 
        
        // Only if timer has been started do we keep incrementing it (always lerp from 0)
        if (t > 0)
        {
            t += Time.deltaTime;
        }
        
        // Since lerping is between 0-1, we can reset once it hits one
        if (t > 1.0f)
        {
            t = 0;
        }

        // If we try to zoom closer than orthogSize of 1, just say "no thank you" and stay at 1.
        if (cmv_Camera.m_Lens.OrthographicSize <= 1)
        {
            cmv_Camera.m_Lens.OrthographicSize = 1;
            orthogCameraSize = 1;
        } 
        
        



    }

    public void CameraReset()
    {
        cmv_Camera.Follow = player.GetComponent<Transform>();
    }
}
