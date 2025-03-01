using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : Interactable
{
    public ItemData _data;

    public PickableItem(ItemData newData)
    {
        _data = newData;
    }
    public void Init(ItemData newData)
    {
        _data = newData;
        refName = newData.refName;
        objectName = newData.objectName;
        interactableText = newData.interactableText;
        textWidth = newData.textWidth;
    }
    public override void OnItemInteracted()
    {
        // Disable gameobject
        Debug.Log($"{gameObject.name} || Picked Up");
        Inventory.Instance.OnItemPickUp(this);
        gameObject.SetActive(false);
        // Tell Inventory that this item is picked up
    }
}
