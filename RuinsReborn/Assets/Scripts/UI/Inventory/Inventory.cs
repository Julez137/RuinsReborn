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
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null) Instance = this;
        window = GetComponent<LeanWindow>();
        filterButton.onClick.AddListener(OnFilterButtonClicked);
        equippedItems = inventoryContent.GetComponentsInChildren<EquippedItem>().ToList();
    }
    
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

    void OnFilterButtonClicked()
    {
        filterWindow.EnableWindow();
    }
}
