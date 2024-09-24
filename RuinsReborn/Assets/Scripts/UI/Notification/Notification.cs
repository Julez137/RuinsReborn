using UnityEngine;

public class Notification : MonoBehaviour
{
    public static Notification instance;
    [SerializeField] GameObject notificationPrefab;
    [SerializeField] Transform notificationHolder;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public void PushNotification(PickableItem item, NotificationPrefab.NotificationType notificationType)
    {
        NotificationPrefab newNotification = Instantiate(notificationPrefab, notificationHolder).GetComponent<NotificationPrefab>();
        newNotification.Init(item, notificationType);
    }
}

