using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : Interactable
{
    [Tooltip("The amount of items in this object. This will be multiplied by the base weight to determine total weight")]
    public float itemCount;
    [Tooltip("The amount a single count of this item weighs in Kg's. This will affect the amount of weight the player is carrying.")]
    public float weight;
    [Tooltip("The total weight in Kg's. This is set automatically in runtime / on initialisation")]
    public float totalWeight;
    [Tooltip("The item's visual icon when shown in inventory.")]
    public Sprite uiSprite;

    public override void OnItemInteracted()
    {
        // Disable gameobject
        Debug.Log($"{gameObject.name} || Picked Up");
        Notification.instance.PushNotification(this, NotificationPrefab.NotificationType.PickUp);
        gameObject.SetActive(false);
        // Tell Inventory that this item is picked up
    }
}
