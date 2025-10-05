using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EncapsulatedItem assignedItem;

    /// <summary>
    /// This updates any visuals needed for the child class when its hovered
    /// </summary>
    /// 
    public abstract void OnItemHover();
    
    /// <summary>
    /// This updates any visuals needed for the child class when its not hovered
    /// </summary>
    public abstract void OnItemLeft();
    
    /// <summary>
    /// Assigns the item to the child class
    /// </summary>
    /// <param name="item">Encapsulated item that will be assigned</param>
    public abstract void OnItemAssigned(EncapsulatedItem item);
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnItemHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnItemLeft();
    }
}
