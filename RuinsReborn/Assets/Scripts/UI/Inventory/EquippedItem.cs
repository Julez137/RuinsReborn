using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItem : MonoBehaviour
{
    [SerializeField] EncapsulatedItem encapsulatedItemPrefab;
    [SerializeField] Transform itemHolder;
    PickableItem thisAssignedItem;

    [SerializeField] private List<ItemPair> itemList = new List<ItemPair>();
    Dictionary<ItemData, EncapsulatedItem> items = new Dictionary<ItemData, EncapsulatedItem>();

    public void AddItem(ItemData newItem)
    {
        if (newItem == null || items.ContainsKey(newItem))
            return;

        // Instantiate the encapsulatedItemPrefab in the scene
        EncapsulatedItem newEncapsulation = Instantiate(encapsulatedItemPrefab, itemHolder);
        newEncapsulation.Init(newItem);

        // Add the new encapsulated item to the dictionary
        items.Add(newItem, newEncapsulation);

        // Sync list for visibility in Inspector
        itemList.Add(new ItemPair { key = newItem, value = newEncapsulation });
    }
    /// <summary>
    /// Refreshes this item's encapsulated items based on the filter of the header
    /// </summary>
    public void RefreshItems()
    {

    }
}

[Serializable]
public class ItemPair
{
    public ItemData key;
    public EncapsulatedItem value;
}
