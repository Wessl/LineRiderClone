using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Apply this to any button that needs mouse over tooltip behavior
public class ButtonTooltipHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea] [SerializeField] private string tooltipText;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Showtooltip_Static(tooltipText);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Hidetooltip_Static();
    }
}
