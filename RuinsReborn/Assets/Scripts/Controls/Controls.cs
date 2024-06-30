using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Controls : MonoBehaviour
{
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

    private void Update()
    {
        // Inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMenuOpen = Inventory.Instance.InventoryPressed();
            UpdateCursor();
        }
    }

    void UpdateCursor()
    {
        Cursor.lockState = isMenuOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isMenuOpen;
        firstPersonController.m_MouseLook.XSensitivity = isMenuOpen ? 0f : xLookSensitivity;
        firstPersonController.m_MouseLook.YSensitivity = isMenuOpen ? 0f : yLookSensitivity;
    }
}
