using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIControls : MonoBehaviour
{
    private KeyBinds keyBinds;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private GameObject hoveredUIObject; // The UI object currently hovered

    private PointerEventData pointerEventData;
    
    KeyCode lastKeyPressed = KeyCode.None;
    private void Awake()
    {

        // Fallback: Try to find components automatically
        if (raycaster == null)
            raycaster = FindObjectOfType<GraphicRaycaster>();
        if (eventSystem == null)
            eventSystem = EventSystem.current;
    }

    private void Start()
    {
        keyBinds = Resources.Load<KeyBinds>("ScriptableObjects/KeyBinds");
    }

    private void Update()
    {
        // Don't do anything if a UI menu isn't open
        if (!IsMenuOpen()) return;

        // Create PointerEventData based on current mouse position
        AssignPointerData();
        
        // Move the HeldItem to were the mouse is pointing
        SetHeldItemPosition();

        // Raycast to find UI elements under the pointer
        GetHoveredObject();

        // Don't continue if there's no UI element hovered
        if (hoveredUIObject == null) return;

        // Check if the input Keycode exist in Unity Enum Directory
        GetInput();

        // If the key that was pressed or lifted exists in Unity Enum Directory, do continue.
        if (lastKeyPressed == KeyCode.None) return;
        
        // Check to see if the comparer key exists in assigned UI Keybinds list.
        bool doesExist = keyBinds.DoesKeyBindExist(KeyBinds.KeyBindType.UI, lastKeyPressed);
        
        // Continue if that comparer key does exist in the UI Keybinds list
        if(!doesExist) return;

        // Go through all possible keybind actions
        CheckInputs();
    }
    
    #region Update Methods

    bool IsMenuOpen()
    {
        if (Controls.Instance == null) Debug.Log("Controls singleton is null");
        return Controls.Instance.isMenuOpen;
    }

    void AssignPointerData()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
    }

    void SetHeldItemPosition()
    {
        if (HeldItem.Instance.IsHoldingItem())
            HeldItem.Instance.SetPosition(pointerEventData.position);
    }

    void GetHoveredObject()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            hoveredUIObject = results[0].gameObject;
        }
        else
        {
            hoveredUIObject = null;
        }
    }

    void GetInput()
    {
        lastKeyPressed = KeyCode.None;
        if (Input.anyKeyDown) // Only runs if *some* key was pressed
        {
            // Check all keycodes only once when a key is pressed
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    lastKeyPressed = key;
                    break; // Stop after finding the first key
                }
            }
        }
        else
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyUp(key))
                {
                    lastKeyPressed = key;
                    break; // Stop after finding the first key
                }
            }
        }
    }

    void CheckInputs()
    {
        // If the Drop_Item keybind is pressed
        if (Input.GetKeyDown(keyBinds.GetKeyBind(KeyBinds.KeyBindType.UI, "Drop Item")))
        {
            DropItem(hoveredUIObject);
            return;
        }

        // Holding an item in inventory
        if (Input.GetKeyDown(keyBinds.GetKeyBind(KeyBinds.KeyBindType.UI, "Select Item")))
        {
            HoldItem(hoveredUIObject);
        }
        
        // Leaving item in inventory
        if (Input.GetKeyUp(keyBinds.GetKeyBind(KeyBinds.KeyBindType.UI, "Select Item")))
        {
            LeaveItem();
        }
    }
    
    #endregion

    #region UI Interaction
    void DropItem(GameObject hoveredItem)
    {
        EncapsulatedItem encapsulatedItem = hoveredItem.GetComponentInParent<EncapsulatedItem>();
        if (encapsulatedItem == null) return;
        
        Inventory.Instance.OnItemDropped(encapsulatedItem, 1);
        
        HotBar.Instance.RefreshItems();
    }

    void HoldItem(GameObject hoveredItem)
    {
        EncapsulatedItem encapsulatedItem = hoveredItem.GetComponentInParent<EncapsulatedItem>();
        if (encapsulatedItem == null) return;
        HeldItem.Instance.HoldItem(encapsulatedItem);
    }

    void LeaveItem()
    {
        ItemSlot itemSlot = hoveredUIObject.GetComponentInParent<ItemSlot>();
        if (itemSlot != null)
        {
            itemSlot.OnItemAssigned(HeldItem.Instance.Item());
        }
        
        HeldItem.Instance.LeaveItem();
    }
    
    #endregion
}
