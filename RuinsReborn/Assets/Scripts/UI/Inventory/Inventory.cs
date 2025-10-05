using System;
using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private LeanWindow window;

    //[Header("Equipment References")]
    [Header("Inventory References")]
    [SerializeField] Button orderByButton;
    [SerializeField] Button filterButton;
    [SerializeField] FilterWindow filterWindow;
    [SerializeField] Transform inventoryContent;
    List<EquippedItem> equippedItems = new List<EquippedItem>();
    
    public EquippedItem[] GetEquippedItems() { return equippedItems.ToArray(); }

    #region Init
    private void Awake()
    {
        if (Instance == null) Instance = this;
        window = GetComponent<LeanWindow>();
        filterButton.onClick.AddListener(OnFilterButtonClicked);
        equippedItems = inventoryContent.GetComponentsInChildren<EquippedItem>().ToList();
    }

    private void Start()
    {
        HotBar.Instance.RefreshSlots();
    }

    #endregion
    
    #region Public methods
    
    public bool InventoryPressed()
    {
        if (window.On)
        {
            window.TurnOff();
            // Inventory should close after Inventory button is pressed - Update isMenuOpen to false
            return false;
        }
        else
        {
            window.TurnOn();
            // Inventory should open after Inventory button is pressed - Update isMenuOpen to true
            return true;
        }
    }

    public void OnItemPickUp(PickableItem item)
    {
        // Store Item in inventory
        equippedItems[0].AddItem(item._data);
        Notification.instance.PushNotification(item, NotificationPrefab.NotificationType.PickUp);
    }

    public void OnItemDropped(EncapsulatedItem encapsulatedItem, int dropCount)
    {
        encapsulatedItem.DropItem();
        HotBar.Instance.RefreshItems();
    }

    #endregion

    #region Helper Methods

    void OnFilterButtonClicked()
    {
        filterWindow.EnableWindow();
    }

    #endregion
    
}
