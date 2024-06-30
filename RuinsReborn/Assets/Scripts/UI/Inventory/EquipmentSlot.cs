using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image equipmentIcon;
    [SerializeField] Image bodyHighlight;
    [SerializeField] Color highlightColor;

    private void Start()
    {
        equipmentIcon.enabled = false;
        if (bodyHighlight == null) return;
        bodyHighlight.enabled = false;
    }

    public void OnHighlight(bool isHighlighted)
    {
        if (bodyHighlight == null) return;
        bodyHighlight.enabled = isHighlighted;
        bodyHighlight.color = highlightColor;
    }
}
