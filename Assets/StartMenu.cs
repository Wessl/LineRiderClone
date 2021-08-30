using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Camera camera;
    public Color lowRed;
    public Color lowBlue;
    public void ClickPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void FixedUpdate()
    {
        Color color = Color.LerpUnclamped(lowRed, lowBlue, Mathf.Sin((Time.time + Mathf.PI/4)));

        camera.backgroundColor = color;
    }
}
