using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class HotBarSlot : ItemSlot
{
    [SerializeField] Image icon;
    [SerializeField] Sprite lockedSprite;
    [SerializeField] bool isLocked = false;
    [SerializeField] GameObject selectionBorders;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private HotBarState hotBarState = HotBarState.Empty;
    [SerializeField] private HotbarStateColor[] _hotBarStateColors;

    private Image _thisImage;
    private readonly Color _defaultColor = new Color(0f, 0f, 0f, 0.58f);

    public HotBarState GetState() { return hotBarState;}

    public void SetLocked(bool isLocked)
    {
        Debug.Log($"[{gameObject.name}] Lock : {isLocked}");
        _thisImage = GetComponent<Image>();
        _thisImage.color = _defaultColor;
        this.isLocked = isLocked;
        icon.sprite = isLocked ? lockedSprite : null;
        hotBarState = isLocked ? HotBarState.Locked : HotBarState.Empty;
        SetHotbarState(hotBarState);
    }

    void SetItem(ItemData newData)
    {
        data = newData;
        if (data == null)
        {
            Debug.Log($"[{gameObject.name}] Item null. Set Hotbar state to Empty");
            SetHotbarState(HotBarState.Empty);
            icon.sprite = null;
        }
        else
        {
            Debug.Log($"[{gameObject.name}] Item assigned : {data.objectName}. Set Hotbar state to Assigned");
            SetHotbarState(HotBarState.Assigned);
            icon.sprite = data.uiSprite;
        }
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

    public override void OnItemHover()
    {
        if (!IsValid()) return;
        _thisImage.color = ColorHandling.HoverColor;
    }

    public override void OnItemLeft()
    {
        _thisImage.color = _defaultColor;
    }

    public override void OnItemAssigned(ItemData data)
    {
        if (!IsValid()) return;

        SetItem(data);
    }

    bool IsValid()
    {
        if (!HeldItem.Instance.IsHoldingItem()) return false;
        if (isLocked) return false;

        return true;
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
