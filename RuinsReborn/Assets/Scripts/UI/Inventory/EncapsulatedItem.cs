using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncapsulatedItem : MonoBehaviour
{
    EquippedItem equippedItem;
    ItemData thisItem;

    [SerializeField] Image imgItemIcon;
    [SerializeField] TextMeshProUGUI textItemName;
    [SerializeField] TextMeshProUGUI textItemCount;
    [SerializeField] TextMeshProUGUI textItemWeight;
    public void Init(EquippedItem equippedItem, ItemData data)
    {
        this.equippedItem = equippedItem;

        UpdateNewData(data);
    }

    public void UpdateNewData(ItemData data)
    {
        if (data.itemCount <= 0)
        {
            equippedItem.RemoveItem(this);
            return;
        } 

        thisItem = data;
        imgItemIcon.sprite = data.uiSprite;
        textItemName.text = data.objectName;
        textItemCount.text = data.itemCount.ToString();
        textItemWeight.text = $"{data.weight} ({data.totalWeight})";
    }

    public void DropItem(int dropCount = 1)
    {
        WorldBuilderManager.instance.DropItem(new ItemData(thisItem), dropCount);
        thisItem.RemoveItem(dropCount);
        UpdateNewData(thisItem);
    }
}
