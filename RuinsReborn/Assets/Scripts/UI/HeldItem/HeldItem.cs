using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeldItem : MonoBehaviour
{
    public static HeldItem Instance;
    [SerializeField] private Image imageItem;
    private ItemData heldItem;
    
    private RectTransform rectTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        rectTransform = GetComponent<RectTransform>();
    }

    public void HoldItem(ItemData itemData)
    {
        heldItem = itemData;
        imageItem.sprite = heldItem.uiSprite;
        imageItem.gameObject.SetActive(true);
        Cursor.visible = false;
    }

    public void LeaveItem()
    {
        heldItem = null;
        imageItem.sprite = null;
        imageItem.gameObject.SetActive(false);
        Cursor.visible = true;
    }

    public void SetPosition(Vector2 position)
    {
        // Set the UI element's position to the mouse position
        rectTransform.position = position;
    }
}
