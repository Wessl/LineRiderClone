using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private static Tutorial instance;
    
    [SerializeField]
    private Camera uiCamera;

    public TextMeshProUGUI tooltipTMP;
    public RectTransform backgroundRectTransform;

    [SerializeField] [TextArea]
    private string[] tutorialText;

    [SerializeField] private Vector3[] positionsOfText;

    private int iterator;

    private void Awake()
    {
        instance = this;
        iterator = 0;
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        ShowTooltip(tutorialText[iterator]);
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition,
            uiCamera, out localPoint);
        //transform.localPosition = localPoint;
    }

    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);
        tooltipTMP.SetText(tooltipString);
        tooltipTMP.ForceMeshUpdate();
        gameObject.transform.position = positionsOfText[iterator];
        float textPadding = 4f;
        Vector2 backgroundSize = tooltipTMP.GetRenderedValues();
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = backgroundSize + paddingSize;
        iterator++;
    }

    public void ClickNextButton()
    {
        if (iterator >= tutorialText.Length)
        {
            // Tutorial finished
            gameObject.SetActive(false);
            // Let's never show it again
            PlayerPrefs.SetString("TutorialDone", "Yes");
        }
        else
        {
            ShowTooltip(tutorialText[iterator]);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var pos in positionsOfText)
        {
            Debug.Log("drawing gizmos");
            Gizmos.DrawSphere(pos, 15f);
        }
    }
}
