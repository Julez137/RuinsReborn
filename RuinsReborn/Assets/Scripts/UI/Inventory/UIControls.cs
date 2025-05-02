using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIControls : MonoBehaviour
{
    public UIBinds[] uiBinds;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public GameObject hoveredUIObject; // The UI object currently hovered

    private PointerEventData pointerEventData;
    private void Awake()
    {

        // Fallback: Try to find components automatically
        if (raycaster == null)
            raycaster = FindObjectOfType<GraphicRaycaster>();
        if (eventSystem == null)
            eventSystem = EventSystem.current;
    }

    private void Update()
    {
        // Don't do anything if a UI menu isn't open
        if (Controls.instance == null) Debug.Log("Controls singleton is null");
        if (!Controls.instance.isMenuOpen) return;

        // Create PointerEventData based on current mouse position
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

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

        // Don't continue if the Drop_Item  keycode is not found
        if (GetUIBindKey("Drop_Item") == KeyCode.Ampersand) return;
        // If the Drop_Item keybind is pressed
        if (Input.GetKeyDown(GetUIBindKey("Drop_Item")))
        {
            DropItem(hoveredUIObject);
        }
    }

    void DropItem(GameObject hoveredItem)
    {

        EncapsulatedItem encapsulatedItem = hoveredItem.GetComponentInParent<EncapsulatedItem>();
        if (encapsulatedItem == null) return;
        encapsulatedItem.DropItem();
    }

    public KeyCode GetUIBindKey(string comparer)
    {
        foreach (var itembind in uiBinds)
        {
            if (itembind.keyName == comparer) 
                return itembind.key;
        }

        return KeyCode.Ampersand;
    }

    [System.Serializable]
    public class UIBinds
    {
        public string keyName;
        public KeyCode key;
    }
}
