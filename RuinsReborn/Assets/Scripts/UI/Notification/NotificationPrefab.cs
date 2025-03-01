using Lean.Transition;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPrefab : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] TextMeshProUGUI itemAmount;
    [SerializeField] Color pickUpColor;
    [SerializeField] Color dropColor;
    [SerializeField] LeanManualAnimation displayAnimation;

    public void Init(PickableItem item, NotificationType notificationType)
    {
        icon.sprite = item._data.uiSprite;
        itemText.text = item.objectName;

        string character = "";
        if (notificationType == NotificationType.PickUp)
            character = "+";
        else
            character = "-";

        Color newColor = Color.white;
        if (notificationType == NotificationType.PickUp)
            newColor = pickUpColor;
        else
            newColor = dropColor;

        itemAmount.text = $"{character}{item._data.itemCount}";
        itemAmount.color = newColor;

        displayAnimation.BeginTransitions();
    }

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }

    public enum NotificationType
    {
        PickUp,
        Drop
    }
}


