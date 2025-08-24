using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Controls : MonoBehaviour
{
    public static Controls instance;
    public Camera mainCamera;
    [Header("Control Layouts")]
    public KeyCode inventoryKey;
    public KeyCode interactKey;

    [Header("Raycast Settings")]
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] float raycastDistance;
    [SerializeField] float pickUpRadius;
    [Header("Runtime Info")]
    // Used to determine whether a menu is open to make sure the player can't use controls outside of a UI menu.
    public bool isMenuOpen = false;
    [SerializeField] Interactable closestItem;
    [SerializeField] Collider[] possibleItems;
    FirstPersonController firstPersonController;
    private float xLookSensitivity = 0f;
    private float yLookSensitivity = 0f;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();
        if (firstPersonController == null)
        {
            Debug.LogWarning($"{gameObject.name} || Controls - FirstPersonController not found. Certain actions involving the camera won't work");
        }
        else
        {
            xLookSensitivity = firstPersonController.MouseLook.XSensitivity;
            yLookSensitivity = firstPersonController.MouseLook.YSensitivity;
        }
        ToolTip.instance.HideText();
        this.DelayFrames(1, () => UpdateCursor());
    }

    private void OnDrawGizmosSelected()
    {
        if (firstPersonController == null) return;
        if (firstPersonController.FovKick.Camera == null) return;
        Vector3 pos = firstPersonController.FovKick.Camera.transform.position;
        Vector3 dir = firstPersonController.FovKick.Camera.transform.forward * 5;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pos, dir);
    }

    private void Update()
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
            firstPersonController.MouseLook.XSensitivity = isMenuOpen ? 0f : xLookSensitivity;
            firstPersonController.MouseLook.YSensitivity = isMenuOpen ? 0f : yLookSensitivity;
        }
    }

    void LookForInteractable()
    {
        // Check if the fps Controller is referenced
        if (firstPersonController == null) return;
        // Check if the menu is open or if the camera is referenced
        if (isMenuOpen || firstPersonController.FovKick.Camera == null) return;
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

                // Don't continue if there is no interactable item in the interactable radius and hide tooltip
                if (closestItem == null)
                {
                    ToolTip.instance.HideText();
                    return;
                }
                
                // Tell the pop-op UI to show instruction to pick up item
                closestItem.OnItemLook(interactKey);

                // Interact with items - for debugging
                if (Input.GetKeyDown(interactKey))
                {
                    //Debug.Log($"{gameObject.name} || Interact key pressed");
                    if (isMenuOpen) return;
                }
            }
        }
        else ToolTip.instance.HideText();
    }
}
