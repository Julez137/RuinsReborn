using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GlobalItemsDataHolder", menuName = "ScriptableObjects/GlobalItemsDataHolder")]
public class GlobalItemsDataHolder : ScriptableObject
{
    public ItemData[] items;
}

[System.Serializable]
public class ItemData
{
    [Header("Interactable Class")]
    [Tooltip("The name used to reference the gameobject in the scene. This is only to assign the data to the matched object, not for UI.")]
    public string refName;
    [Tooltip("The name of the object. This will be displayed in UI menus")]
    public string objectName;
    [Tooltip("The text being displayed when the object can be interactable. Template: Press <Key> to [insert text here].")]
    public string interactableText;
    [Tooltip("The width of the text displayed. This will control the background of the text to highlight it when displayed.")]
    public float textWidth;
    
    [Header("PickableItem Class")]
    [Tooltip("The amount a single count of this item weighs in Kg's. This will affect the amount of weight the player is carrying.")]
    public float weight;
    [Tooltip("The total amount this item weighs in Kg's. This will affect the amount of weight the player is carrying.")]
    public float totalWeight;
    [Tooltip("The item's visual icon when shown in inventory.")]
    public Sprite uiSprite;
    [Tooltip("The amount of items in this object. This will be multiplied by the base weight to determine total weight")]
    public int itemCount;
    [Tooltip("If the item is stackable in the inventory")]
    public bool isStackable;

    public bool IsEquals(ItemData other)
    {
        return objectName == other.objectName;
    }

    public void AddItem(int count)
    {
        itemCount += count;
        Refresh();
    }

    public void RemoveItem(int count)
    {
        itemCount -= count;
        Refresh();
    }

    public void Refresh()
    {
        if (itemCount <= 0)
        {
            // Delete itself, as the item does not exist anymore
            return;
        }

        totalWeight = weight * itemCount;
    }

    public ItemData(ItemData newdata)
    {
        refName = newdata.refName;
        objectName = newdata.objectName;
        interactableText = newdata.interactableText;
        textWidth = newdata.textWidth;
        weight = newdata.weight;
        totalWeight = newdata.totalWeight;
        uiSprite = newdata.uiSprite;
        itemCount = newdata.itemCount;
        isStackable = newdata.isStackable;
    }
}
