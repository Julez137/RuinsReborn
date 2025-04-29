using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncapsulatedItem : MonoBehaviour
{
    ItemData thisItem;

    [SerializeField] Image imgItemIcon;
    [SerializeField] TextMeshProUGUI textItemName;
    [SerializeField] TextMeshProUGUI textItemCount;
    [SerializeField] TextMeshProUGUI textItemWeight;
    public void Init(ItemData data)
    {
        if (data.itemCount <= 0) Destroy(gameObject);

        thisItem = data;
        imgItemIcon.sprite = data.uiSprite;
        textItemName.text = data.objectName;
        textItemCount.text = data.itemCount.ToString();
        textItemWeight.text = $"{data.weight} ({data.totalWeight})";
    }

    public void DropItem(int dropCount = 1)
    {
        thisItem.RemoveItem(dropCount);
        Init(thisItem);
    }
}
