using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData data;
    public abstract void OnItemHover();
    public abstract void OnItemLeft();
    public abstract void OnItemAssigned(ItemData data);
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        bool isHoldingItem = HeldItem.Instance.IsHoldingItem();
        ItemData itemData = HeldItem.Instance.Data();

        // if (isHoldingItem)
        //     Debug.Log($"[{gameObject.name}] Pointer Enter. Is holding item : {itemData.objectName}");
        // else
        //     Debug.Log($"[{gameObject.name}] Pointer Enter. Not holding an Item");
        
        OnItemHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bool isHoldingItem = HeldItem.Instance.IsHoldingItem();
        ItemData itemData = HeldItem.Instance.Data();

        // if (isHoldingItem)
        //     Debug.Log($"[{gameObject.name}] Pointer Exit. Is holding item : {itemData.objectName}");
        // else
        //     Debug.Log($"[{gameObject.name}] Pointer Exit. Not holding an Item");
        
        OnItemLeft();
    }
}
