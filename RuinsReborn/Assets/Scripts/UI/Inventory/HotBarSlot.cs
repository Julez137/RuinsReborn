using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Sprite lockedSprite;
    [SerializeField] bool isLocked;
    [SerializeField] GameObject selectionBorders;

    public void SetLocked(bool isLocked)
    {
        this.isLocked = isLocked;
        icon.sprite = lockedSprite;
    }

    public void SetItem(PickableItem item)
    {
        icon.sprite = item.uiSprite;
    }

    public void EnableSelectionBorders(bool isEnabled)
    {
        selectionBorders.SetActive(isEnabled);
    }
}
