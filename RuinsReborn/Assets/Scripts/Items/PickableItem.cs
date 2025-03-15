using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : Interactable
{
    public ItemData _data;

    public override void Init(ItemData newData)
    {
        base.Init(newData);
        _data = newData;
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
