using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuOptions : MonoBehaviour
{
    public LineEraser _lineEraser;

    public Slider eraserSlider;
    public GameObject lineNormal;
    public GameObject lineBoost;
    public GameObject lineBounce;

    public GameObject saveLevelDialogue;
    public TMP_InputField levelNameInputField;
    public GameObject errorText;

    public GameObject loadFileCell;
    public GameObject cellParent;

    public GameObject loadFilesPanel;
    public GameObject closeLoadFilesPanelButton;
    private List<String> filesNames;
    private List<GameObject> cells;
    public GameObject deletionConfirmWindow;

    private int deletionIndex;

    private void Start()
    {
        cells = new List<GameObject>();
    }

    public void SetEraserWidth()
    {
        _lineEraser.EraseRadius = eraserSlider.value;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SaveLines()
    {
        // Bring up dialogue window
        saveLevelDialogue.SetActive(true);
        errorText.SetActive(false);
    }

    public void ConfirmLevel()
    {
        string inputAttempt = levelNameInputField.text;
        
        // Check if there is anything to save first
        if (GameObject.FindGameObjectsWithTag("Line").Length == 0)
        {
            ActivateConfirmLevelErrorText("There is nothing to save!");
        }
        // Validate file name
        else if (inputAttempt.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
        {
            ActivateConfirmLevelErrorText("Invalid name!");
        } 
        else if (inputAttempt.Length == 0)
        {
            ActivateConfirmLevelErrorText("Must have a name!");

        }
        else if (System.IO.File.Exists(SaveSystem.MakeLineRiderSaveFileName(inputAttempt)))
        {
            ActivateConfirmLevelErrorText("Level with that name exists already!");
        }
        else
        {
            // Valid file name, can continue
            Line[] lines = GameObject.FindObjectsOfType<Line>();
            SaveSystem.SaveLines(lines, inputAttempt);
            saveLevelDialogue.SetActive(false);
        }
    }

    private void ActivateConfirmLevelErrorText(string errorMsg)
    {
        errorText.SetActive(true);
        errorText.GetComponent<TextMeshProUGUI>().text = errorMsg;
    }

    public void LoadLines(string fileNameInput)
    {
        // Before loading in new lines, remove the old ones!
        RemoveOldLines();
        
        LineData lineData = SaveSystem.LoadLines(fileNameInput);
        var lines = lineData.LineRenderers;
        var lineTypes = lineData.LineTypes;
        for (int i = 0; i < lines.Count; i++)
        {
            string lineType = lineTypes[i];
            GameObject typeToInstantiate;
            switch (lineType)
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
            GameObject newLine = Instantiate(typeToInstantiate, new Vector2(0,0), Quaternion.identity);
            
            // Turn line containing float[] into Vector2[]
            double[] allPointValues = lines[i];
            Vector2[] points = new Vector2[allPointValues.Length / 3];
            for (int y = 0; y < allPointValues.Length; y+=3)
            {
                points[y/3] = new Vector2((float)allPointValues[y], (float)allPointValues[y + 1]);
            }
            
            newLine.GetComponent<Line>().ConstructLineFromPoints(points);
        }
    }

    private void RemoveOldLines()
    {
        var allLines = GameObject.FindGameObjectsWithTag("Line");
        foreach (var line in allLines)
        {
            Destroy(line);
        }
    }

    public void OnInputFieldChange()
    {
        errorText.SetActive(false);
    }

    // Cancels the saving of a level
    public void CancelSaveLevelDialogueButton()
    {
        saveLevelDialogue.SetActive(false);
    }

    public void LoadDataOuterButton()
    {
        // Reset old list of files, destroy stuff so potentially old stuff doesnt stay
        filesNames = new List<string>();
        foreach (var oldCell in cells)
        {
            Destroy(oldCell);
        }
        // Look in the Application.PersistentDataPath
        loadFilesPanel.SetActive(true);
        closeLoadFilesPanelButton.SetActive(true);
        string location = Application.persistentDataPath;
        string[] filesNamesTemp = Directory.GetFiles(location);
        int i = 0;
        foreach (var fileName in filesNamesTemp)
        {
            // Debug.Log("this filename was found: " + fileName);
            var cell = Instantiate(loadFileCell, Vector3.zero, Quaternion.identity, cellParent.transform);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            cell.GetComponentInChildren<TextMeshProUGUI>().text = fileNameWithoutExtension;
            cells.Add(cell);
            // Also add to list of filenames so we can access it later
            filesNames.Add(fileNameWithoutExtension);
            // Set up button with correct onClick event, where index corresponds to index of file being loaded
            Button[] button = cell.GetComponentsInChildren<Button>();
            int x = new int();
            x = i++;
            button[0].onClick.AddListener(delegate { LoadFile(x); });
            button[1].onClick.AddListener(delegate { DeleteFile(x); });
        }
    }

    public void LoadFile(int index)
    {
        LoadLines(filesNames[index]);
    }

    public void BringUpDeletionWindow()
    {
        deletionConfirmWindow.SetActive(true);
    }

    public void CancelDeletion()
    {
        deletionConfirmWindow.SetActive(false);
    }

    public void ConfirmDeletion()
    {
        string path = SaveSystem.MakeLineRiderSaveFileName(filesNames[deletionIndex]);
        try
        {
            System.IO.File.Delete(path);
            // Once file has been deleted, reload
            LoadDataOuterButton();
        }
        catch (Exception e)
        {
            Debug.LogError("Tried deleting file and failed. " + e);
        }
        deletionConfirmWindow.SetActive(false);
    }

    public void DeleteFile(int index)
    {
        deletionIndex = index;
        BringUpDeletionWindow();
    }

    public void CloseLoadPanel()
    {
        loadFilesPanel.SetActive(false);
        closeLoadFilesPanelButton.SetActive(false);
    }
    
}
