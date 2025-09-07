using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    public static HotBar Instance;
    [SerializeField] private HotBarSlot[] slots;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        slots = GetComponentsInChildren<HotBarSlot>(true);

        foreach (var slot in slots)
        {
            slot.SetLocked(true);
        }
    }

    public void Refresh()
    {
        Debug.Log($"Refresh Hotbar");
        EquippedItem[] equippedItems = Inventory.Instance.GetEquippedItems();
        int openHotbarSlots = 0;
        foreach (var equippedItem in equippedItems)
        {
            openHotbarSlots += equippedItem.GetItemData().hotbarSlotsCount;
        }

        Debug.Log($"Open Hotbar Slots : {openHotbarSlots}");
        int currentSlot = 0;
        foreach (var slot in slots)
        {
            if (currentSlot < openHotbarSlots)
            {
                Debug.Log($"Open Slot : {slot.gameObject.name}");
                slot.SetLocked(false);
            }
            else
            {
                Debug.Log($"Lock Slot : {slot.gameObject.name}");
                slot.SetLocked(true);
            }

            currentSlot++;
        }
    }

    public void SelectSlot(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            HotBarSlot currentSlot = slots[i];
            
            if (currentSlot.GetState() == HotBarState.Locked) continue;
            
            currentSlot.IsSelected(index == i);
        }
    }
}
