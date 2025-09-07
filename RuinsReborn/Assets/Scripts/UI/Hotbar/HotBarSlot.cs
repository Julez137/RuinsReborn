using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Sprite lockedSprite;
    [SerializeField] bool isLocked = false;
    [SerializeField] GameObject selectionBorders;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private HotBarState hotBarState = HotBarState.Empty;
    [SerializeField] private HotbarStateColor[] _hotBarStateColors;

    public HotBarState GetState() { return hotBarState;}

    public void SetLocked(bool isLocked)
    {
        this.isLocked = isLocked;
        icon.sprite = isLocked ? lockedSprite : null;
        hotBarState = isLocked ? HotBarState.Locked : HotBarState.Empty;
        SetHotbarState(hotBarState);
    }

    public void SetItem(PickableItem item)
    {
        icon.sprite = item._data.uiSprite;
    }

    public void IsSelected(bool isSelected)
    {
        // If the hotbar slot is selected already, disable it to mimic sheathing an item
        if (this.isSelected && isSelected)
        {
            this.isSelected = false;
            selectionBorders.SetActive(false);
            return;
        }
        
        this.isSelected = isSelected;
        selectionBorders.SetActive(isSelected);
    }

    public void SetHotbarState(HotBarState state)
    {
        hotBarState = state;

        foreach (var stateColor in _hotBarStateColors)
        {
            if (stateColor.state == state)
            {
                icon.color = stateColor.color;
                break;
            }
        }
    }
}

public enum HotBarState
{
    Empty,
    Assigned,
    Locked
}

[Serializable]
public struct HotbarStateColor
{
    public HotBarState state;
    public Color color;
}
