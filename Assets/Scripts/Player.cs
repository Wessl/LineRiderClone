using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector3 ogPos;
    private Quaternion ogRot;
    private bool hasStarted;
    public TextMeshProUGUI startTextToRemoveOnPlay;
    private void Update()
    {
        if (Input.GetButtonDown("Play"))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            // Reset camera in case it has been moved
            GameObject.FindObjectOfType<CameraControlOverride>().CameraReset();
            startTextToRemoveOnPlay.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        ogPos = gameObject.transform.position;
        ogRot = gameObject.transform.rotation;
        hasStarted = false;
    }

    public void ResetPosition()
    {
        transform.position = ogPos;
        // And rotation
        transform.localRotation = ogRot;
        // Also reset physics
        rb.bodyType = RigidbodyType2D.Static;
        // And ze camera
        GameObject.FindObjectOfType<CameraControlOverride>().CameraReset();
    }
}
