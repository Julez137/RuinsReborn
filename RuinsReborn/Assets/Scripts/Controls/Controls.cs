using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Controls : MonoBehaviour
{
    public static Controls Instance;
    public Camera mainCamera;

    private KeyBinds _keyBinds;

    [Header("Raycast Settings")]
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] float raycastDistance;
    [SerializeField] float pickUpRadius;
    [Header("Runtime Info")]
    // Used to determine whether a menu is open to make sure the player can't use controls outside of a UI menu.
    public bool isMenuOpen = false;
    [SerializeField] Interactable closestItem;
    [SerializeField] Collider[] possibleItems;
    FirstPersonController _firstPersonController;
    private Interactable _lastLookedAtInteractable;
    private float _xLookSensitivity = 0f;
    private float _yLookSensitivity = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;

    }
    private void Start()
    {
        _keyBinds = Resources.Load<KeyBinds>("ScriptableObjects/KeyBinds");
        
        _firstPersonController = GetComponent<FirstPersonController>();
        if (_firstPersonController == null)
        {
            Debug.LogWarning($"{gameObject.name} || Controls - FirstPersonController not found. Certain actions involving the camera won't work");
        }
        else
        {
            _xLookSensitivity = _firstPersonController.MouseLook.XSensitivity;
            _yLookSensitivity = _firstPersonController.MouseLook.YSensitivity;
        }
        ToolTip.instance.HideText();
        this.DelayFrames(1, () => UpdateCursor());
    }

    private void OnDrawGizmosSelected()
    {
        if (_firstPersonController == null) return;
        if (_firstPersonController.FovKick.Camera == null) return;
        Vector3 pos = _firstPersonController.FovKick.Camera.transform.position;
        Vector3 dir = _firstPersonController.FovKick.Camera.transform.forward * 5;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pos, dir);
    }

    private void Update()
    {
        LookForInteractable();
        if (Input.anyKeyDown) // Check if any key is pressed
        {
            // Store the currently pressed button
            KeyCode lastKeyPressed = KeyCode.None;
            // Check all possible keycodes and store keycode if valid
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    lastKeyPressed = key;
                    break; // Stop after finding the key
                }
            }
            
            if (lastKeyPressed == KeyCode.None) return;
            
            // Inventory
            if (Input.GetKeyDown(_keyBinds.GetKeyBind(KeyBinds.KeyBindType.Normal, "Inventory")))
            {
                isMenuOpen = Inventory.Instance.InventoryPressed();
                UpdateCursor();
                return;
            }

            // Hotbar
            if (_keyBinds.DoesKeyBindExist(KeyBinds.KeyBindType.Hotbar, lastKeyPressed))
            {
                // Select the associated hotbar slot
                HotBar.Instance.SelectSlot(_keyBinds.GetKeycodeIndexInList(KeyBinds.KeyBindType.Hotbar, lastKeyPressed));
                return;
            }
            
            // Drop Item
            if (Input.GetKeyDown(_keyBinds.GetKeyBind(KeyBinds.KeyBindType.Normal, "Drop Item")))
            {
                if (!isMenuOpen) 
                    HotBar.Instance.DropSelectedItem();
                return;
            }
        }
        
    }

    void UpdateCursor()
    {
        
        Cursor.lockState = isMenuOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isMenuOpen;
        if (_firstPersonController != null)
        {
            _firstPersonController.MouseLook.XSensitivity = isMenuOpen ? 0f : _xLookSensitivity;
            _firstPersonController.MouseLook.YSensitivity = isMenuOpen ? 0f : _yLookSensitivity;
        }
    }

    void LookForInteractable()
    {
        // Check if the fps Controller is referenced
        if (_firstPersonController == null) return;
        // Check if the menu is open or if the camera is referenced
        if (isMenuOpen || _firstPersonController.FovKick.Camera == null) return;
        RaycastHit hit;
        Vector3 pos = mainCamera.transform.position;
        Vector3 dir = mainCamera.transform.forward * 5;

        // Raycast a certain distance to check if it hits anything other than the player
        if (Physics.Raycast(pos, dir, out hit, raycastDistance, ~ignoreLayer))
        {
            // Do nothing if nothing is hit
            if (hit.collider != null)
            {
                float closestDistance = pickUpRadius;
                float currentDistance = 0;
                closestItem = null;
                // Get all items that is in the interactable radius
                possibleItems = Physics.OverlapSphere(hit.point, pickUpRadius, ~ignoreLayer);
                foreach (var item in possibleItems)
                {
                    // Check to see if the currently checked item is a interactable item
                    Interactable possibleItem = item.gameObject.GetComponent<Interactable>();
                    // Skip to next object if this item is not a interactable item
                    if (possibleItem == null)
                    {

                        continue;
                    }

                    // Get current distance between current pickable item and the original hit point
                    currentDistance = Vector3.Distance(hit.point, possibleItem.transform.position);
                    // Set closest item values
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestItem = possibleItem;
                    }
                }

                if (_lastLookedAtInteractable != closestItem)
                {
                    if (_lastLookedAtInteractable != null)
                        _lastLookedAtInteractable.EnableOutline(false);
                    
                    _lastLookedAtInteractable = closestItem;
                    
                }

                // Don't continue if there is no interactable item in the interactable radius and hide tooltip
                if (closestItem == null)
                {
                    ToolTip.instance.HideText();
                    return;
                }
                
                // Tell the pop-op UI to show instruction to pick up item
                closestItem.OnItemLook(_keyBinds.GetKeyBind(KeyBinds.KeyBindType.Normal, "Interact"));

                // Interact with items - for debugging
                if (Input.GetKeyDown(_keyBinds.GetKeyBind(KeyBinds.KeyBindType.Normal, "Interact")))
                {
                    //Debug.Log($"{gameObject.name} || Interact key pressed");
                    if (isMenuOpen) return;
                }
            }
        }
        else ToolTip.instance.HideText();
    }
}
