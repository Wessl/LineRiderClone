using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public CapsuleCollider2D capsuleCollider;
    private Vector3 ogPos;
    private Quaternion ogRot;
    private bool hasStarted;
    public TextMeshProUGUI startTextToRemoveOnPlay;
    private GameObject undoObject;
    public Menu menu;

    private void Update()
    {
        if (Input.GetButtonDown("Play"))
        {
            StartPlaying();
            menu.PlayButton();
        }
    }

    public void StartPlaying()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        // Reset camera in case it has been moved
        GameObject.FindObjectOfType<CameraControlOverride>().CameraReset();
        //startTextToRemoveOnPlay.gameObject.SetActive(false);
    }

    private void Start()
    {
        ogPos = gameObject.transform.position;
        ogRot = gameObject.transform.rotation;
        hasStarted = false;
        
        rb.useAutoMass = true;
        rb.angularDrag = 0;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
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
