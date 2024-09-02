using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Controls : MonoBehaviour
{
    [Header("Control Layouts")]
    public KeyCode inventoryKey;
    public KeyCode interactKey;

    [Header("Raycast Settings")]
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] float raycastDistance;
    [SerializeField] float pickUpRadius;
    [Header("Runtime Info")]
    public bool isMenuOpen = false;
    FirstPersonController firstPersonController;
    private float xLookSensitivity = 0f;
    private float yLookSensitivity = 0f;
    private void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();
        if (firstPersonController == null)
        {
            Debug.LogWarning($"{gameObject.name} || Controls - FirstPersonController not found. Mouse look won't disable on certain actions");
        }
        else
        {
            xLookSensitivity = firstPersonController.m_MouseLook.XSensitivity;
            yLookSensitivity = firstPersonController.m_MouseLook.YSensitivity;
        }
        UpdateCursor();
    }

    private void FixedUpdate()
    {
        LookForInteractable();
        // Inventory
        if (Input.GetKeyDown(inventoryKey))
        {
            isMenuOpen = Inventory.Instance.InventoryPressed();
            UpdateCursor();
        }
    }

    void UpdateCursor()
    {
        Cursor.lockState = isMenuOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isMenuOpen;
        if (firstPersonController != null)
        {
            firstPersonController.m_MouseLook.XSensitivity = isMenuOpen ? 0f : xLookSensitivity;
            firstPersonController.m_MouseLook.YSensitivity = isMenuOpen ? 0f : yLookSensitivity;
        }
    }

    void LookForInteractable()
    {
        // Check if the fps Controller is referenced
        if (firstPersonController == null) return;
        // Check if the menu is open or if the camera is referenced
        if (isMenuOpen || firstPersonController.m_FovKick.Camera == null) return;
        RaycastHit hit;
        Vector3 pos = firstPersonController.m_FovKick.Camera.transform.position;
        Vector3 dir = transform.TransformDirection(firstPersonController.m_FovKick.Camera.transform.forward) * 5;

        // Raycast a certain distance to check if it hits anything other than the player
        if (Physics.Raycast(pos, dir, out hit, raycastDistance, ignoreLayer))
        {
            // Do nothing if nothing is hit
            if (hit.collider != null)
            {
                float closestDistance = pickUpRadius;
                float currentDistance = 0;
                Interactable closestItem = null;
                // Get all items that is in the interactable radius
                Collider[] possibleItems = Physics.OverlapSphere(hit.point, pickUpRadius, ignoreLayer);
                foreach (var item in possibleItems)
                {
                    // Check to see if the currently checked item is a interactable item
                    Interactable possibleItem = item.GetComponent<Interactable>();
                    // Skip to next object if this item is not a interactable item
                    if (possibleItem == null) continue;
                    // Get current distance between current pickable item and the original hit point
                    currentDistance = Vector3.Distance(hit.point, possibleItem.transform.position);
                    // Set closest item values
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestItem = possibleItem;
                    }
                }

                // Don't continue if there is no interactable item in the interactable radius and hide tooltip
                if (closestItem == null)
                {
                    ToolTip.instance.HideText();
                    return;
                }
                
                // Tell the pop-op UI to show instruction to pick up item
                closestItem.OnItemLook(interactKey);

                // Interact with items
                if (Input.GetKeyDown(interactKey))
                {
                    if (isMenuOpen) return;
                }
            }
        }
    }
}
