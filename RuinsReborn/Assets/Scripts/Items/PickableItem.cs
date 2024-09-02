using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : Interactable
{
    [Tooltip("The weight in Kg's. This will affect the amount of weight the player is carrying.")]
    public float weight;
    [Tooltip("The item's visual icon when shown in inventory.")]
    public Sprite uiSprite;

    public override void OnItemInteracted()
    {
        // Disable gameobject
        // Tell Inventory that this item is picked up
    }
}
