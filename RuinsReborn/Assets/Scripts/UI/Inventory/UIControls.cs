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
    private GameObject heldUIObject; // The UI object that is being held

    private PointerEventData pointerEventData;
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
        if (Controls.Instance == null) Debug.Log("Controls singleton is null");
        if (!Controls.Instance.isMenuOpen) return;

        // Create PointerEventData based on current mouse position
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        
        // Move the HeldItem to were the mouse is pointing
        if (heldUIObject != null)
            HeldItem.Instance.SetPosition(pointerEventData.position);

        // Raycast to find UI elements under the pointer
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

        // Don't continue if there's any UI element hovered
        if (hoveredUIObject == null) return;
        
        // Check if the input Keycode exist in binds
        KeyCode lastKeyPressed = KeyCode.None;
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

        if (lastKeyPressed == KeyCode.None) return;
        bool doesExist = keyBinds.DoesKeyBindExist(KeyBinds.KeyBindType.UI, lastKeyPressed);
        
        if(!doesExist) return;

        // If the Drop_Item keybind is pressed
        if (Input.GetKeyDown(keyBinds.GetKeyBind(KeyBinds.KeyBindType.UI, "Drop Item")))
        {
            DropItem(hoveredUIObject);
            return;
        }

        // Holding an item in inventory
        if (Input.GetKeyDown(keyBinds.GetKeyBind(KeyBinds.KeyBindType.UI, "Select Item")))
        {
            heldUIObject = hoveredUIObject;
            HoldItem(heldUIObject);
        }
        
        // Leaving item in inventory
        if (Input.GetKeyUp(keyBinds.GetKeyBind(KeyBinds.KeyBindType.UI, "Select Item")))
        {
            if (heldUIObject == null) return;
            LeaveItem();
            heldUIObject = null;
        }
    }

    void DropItem(GameObject hoveredItem)
    {
        EncapsulatedItem encapsulatedItem = hoveredItem.GetComponentInParent<EncapsulatedItem>();
        if (encapsulatedItem == null) return;
        encapsulatedItem.DropItem();
    }

    void HoldItem(GameObject hoveredItem)
    {
        EncapsulatedItem encapsulatedItem = hoveredItem.GetComponentInParent<EncapsulatedItem>();
        if (encapsulatedItem == null) return;
        HeldItem.Instance.HoldItem(encapsulatedItem.Data());
    }

    void LeaveItem()
    {
        HeldItem.Instance.LeaveItem();
    }
}
