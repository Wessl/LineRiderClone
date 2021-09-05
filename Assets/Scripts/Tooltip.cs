using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;
    
    [SerializeField]
    private Camera uiCamera;

    public TextMeshProUGUI tooltipTMP;
    public RectTransform backgroundRectTransform;

    private void Awake()
    {
        instance = this;
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
        float textPadding = 4f;
        Vector2 backgroundSize = tooltipTMP.GetRenderedValues();
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = backgroundSize + paddingSize;
    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void Showtooltip_Static(string tooltipString)
    {
        instance.ShowTooltip(tooltipString);
    }
    public static void Hidetooltip_Static()
    {
        instance.HideToolTip();

    }
}
