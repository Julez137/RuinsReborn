using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image equipmentIcon;
    [SerializeField] Image bodyHighlight;
    [SerializeField] Color highlightColor;
    [Tooltip("What can be placed in this slot?")]
    [SerializeField] private EquipSlot equipSlot;
    
    public EquipSlot EquipSlot() { return equipSlot; }

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
