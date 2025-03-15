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

    public void AddItem(ItemData newItem)
    {
        if (newItem == null)
            return;

        foreach (var thisItem in itemList)
        {
            ItemData checkedItem = thisItem.item;
            if (checkedItem.IsEquals(newItem) && checkedItem.isStackable)
            {
                // Modify current item with new data
                checkedItem.AddItem(newItem.itemCount);
                thisItem.display.Init(checkedItem);
                return;
            }
        }

        // Instantiate the encapsulatedItemPrefab in the scene
        EncapsulatedItem newEncapsulation = Instantiate(encapsulatedItemPrefab, itemHolder);
        newEncapsulation.Init(newItem);

        // Add new Item to item list
        itemList.Add(new ItemPair { item = newItem, display = newEncapsulation });
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
    public ItemData item;
    public EncapsulatedItem display;
}
