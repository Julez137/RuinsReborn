using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    public static HotBar Instance;
    [SerializeField] private HotBarSlot[] slots;
    private int _selectedSlot = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        slots = GetComponentsInChildren<HotBarSlot>(true);

        foreach (var slot in slots)
        {
            slot.SetLocked(true);
        }
    }

    public void RefreshSlots()
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
            if (slot.assignedItem == null) continue;
            if (slot.assignedItem.Data() == null) continue;
            
            Debug.Log($"[{gameObject.name}] {slot.gameObject.name} item data count : {slot.assignedItem.Data().itemCount}");
            if (slot.assignedItem.Data().itemCount <= 0)
            {
                slot.assignedItem = null;
                slot.SetHotbarState(HotBarState.Empty);
            }
                
        }
    }

    public void SelectSlot(int index)
    {
        bool hasFoundSlot = false;
        for (int i = 0; i < slots.Length; i++)
        {
            HotBarSlot currentSlot = slots[i];
            
            if (currentSlot.GetState() == HotBarState.Locked) continue;

            bool isSelected = index == i;
            
            if (isSelected)
                _selectedSlot = i;

            hasFoundSlot = currentSlot.IsSelected(isSelected);
            
        }

        if (!hasFoundSlot) _selectedSlot = -1;
    }

    public void DropSelectedItem()
    {
        if (_selectedSlot < 0) return;
        if (slots[_selectedSlot].assignedItem == null) return;
            Inventory.Instance.OnItemDropped(slots[_selectedSlot].assignedItem, 1);
    }
}
