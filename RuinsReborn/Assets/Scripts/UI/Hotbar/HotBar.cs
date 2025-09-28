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
        
        // Find all the equipped items in the inventory and get the total amount of open hotbar slots they provide
        EquippedItem[] equippedItems = Inventory.Instance.GetEquippedItems();
        int openHotbarSlots = 0;
        foreach (var equippedItem in equippedItems)
        {
            openHotbarSlots += equippedItem.GetItemData().hotbarSlotsCount;
        }

        // Open up hotbar slots for the amount of slots the equipped items provide
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

    public void RefreshItems()
    {
        // Refresh hotbar slots with assigned items - If theres item data in a slot with 0 or less item counts, set the slots to empty
        foreach (var slot in slots)
        {
            if (slot.GetState() == HotBarState.Locked) continue;
            if (slot.data == null) continue;
            
            Debug.Log($"[{gameObject.name}] {slot.gameObject.name} item data count : {slot.data.itemCount}");
            if (slot.data.itemCount <= 0) 
                slot.SetHotbarState(HotBarState.Empty);
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
