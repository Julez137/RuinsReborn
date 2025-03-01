using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItem : MonoBehaviour
{
    PickableItem thisAssignedItem;
    List<PickableItem> pickableItems = new List<PickableItem>();
    
    /// <summary>
    /// Refreshes this item's encapsulated items based on the filter of the header
    /// </summary>
    public void RefreshItems()
    {

    }
}
