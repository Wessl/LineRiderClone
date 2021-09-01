using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject boostLine;
    public GameObject normalLine;
    public GameObject bounceLine;

    public LineCreator lineCreator;
    public LineEraser lineEraser;
    public Player player;
    public GameObject eraserCursor;
    public GameObject pauseMenu;

    private bool eraserActive = false;
    private bool isPaused = false;
    public Toggle straightLineToggle;

    public Slider timeScaleSlider;
    public TextMeshProUGUI timeScaleTMPRO;

    public void Update()
    {
        // Mid mouse button down turns on eraser
        if (Input.GetMouseButtonDown(1))
        {
            TurnOnEraser();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            TurnOffEraser();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public void SetBoostLine()
    {
        lineCreator.SetLinePrefab(boostLine);
        TurnOffEraser();
    }
    
    public void SetNormalLine()
    {
        lineCreator.SetLinePrefab(normalLine);
        TurnOffEraser();
    }
    
    public void SetBounceLine()
    {
        lineCreator.SetLinePrefab(bounceLine);
        TurnOffEraser();
    }

    public void ResetPlayerPos()
    {
        player.ResetPosition();
    }

    public void ResetEverything()
    {
        SceneManager.LoadScene(1);
        TurnOffEraser();
    }

    public void Erase()
    {
        if (eraserActive)
        {
            // If on, turn off
            TurnOffEraser();
        }
        else
        {
            TurnOnEraser();            
        }
    }

    public void TurnOffEraser()
    {
        // If on, turn off
        eraserActive = false;
        lineEraser.ErasingActive = false;
        lineCreator.DrawingActive = true;
        eraserCursor.SetActive(false);
    }

    public void TurnOnEraser()
    {
        eraserActive = true;
        lineEraser.ErasingActive = true;
        lineCreator.DrawingActive = false;
        eraserCursor.SetActive(true);
    }

    public void PauseMenu()
    {
        if (isPaused)
        {
            // Unpause
            Time.timeScale = timeScaleSlider.value;
            pauseMenu.SetActive(false);
        }
        else
        {
            // Pause
            Time.timeScale = 0;
            // Bring up pause menu
            pauseMenu.SetActive(true);
        }

        isPaused = !isPaused;

    }

    public void ToggleStraightLines()
    {
        if (straightLineToggle.isOn)
        {
            lineCreator.StraightLinesOnly = true;
        }
        else
        {
            lineCreator.StraightLinesOnly = false;

        }
    }

    public void SetTimeScale()
    {
        var val = timeScaleSlider.value;
        Time.timeScale = val;
        timeScaleTMPRO.text = "Time scale: " + val + "x";
    }

    public void ResetTimeScale()
    {
        // Put timescale back to 1.0 by manually moving slider
        timeScaleSlider.value = 1.0f;
        //SetTimeScale(); not needed
    }
    
    
}
