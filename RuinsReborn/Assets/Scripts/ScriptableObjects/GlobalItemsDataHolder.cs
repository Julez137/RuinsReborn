using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Tooltip("The item's visual icon when shown in inventory.")]
    public Sprite uiSprite;
}
