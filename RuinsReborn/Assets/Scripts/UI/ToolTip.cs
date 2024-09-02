using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public static ToolTip instance;
    [SerializeField] TextMeshProUGUI toolTipText;
    [SerializeField] RectTransform toolTipBackground;
    private bool showTootip = false;

    float tooltipHeight;
    private void Awake()
    {
        if (instance == null) instance = this;
        toolTipText.autoSizeTextContainer = true;

        tooltipHeight = toolTipBackground.sizeDelta.y;
    }

    public void ShowText(string message, float width)
    {
        GetComponent<LeanWindow>().TurnOn();
        toolTipBackground.sizeDelta = new Vector2(width, tooltipHeight);
        toolTipText.text = message;
    }

    public void HideText()
    {
        GetComponent<LeanWindow>().TurnOff();
    }
}
