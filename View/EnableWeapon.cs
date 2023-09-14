using UnityEngine;
using Model;
using Utils.injection;

public class EnableWeapon : InjectableBehaviour
{
    [Inject] private NotificationsTemp _notifications;

    [SerializeField] private bool isPlayer;

    void Start()
    {
        _notifications.Add(OnNotification);
        ShowWeapon(null);
    }

    private void OnNotification(NotificationType type, bool player, string value)
    {
        if (type is NotificationType.Reset)
            ShowWeapon(null);
        
        if (isPlayer == player && type is NotificationType.PerformAction)
            ShowWeapon(value);
    }

    private void ShowWeapon(string value)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(child.gameObject.name == value);
    }

    void OnDestroy()
    {
        _notifications.Remove(OnNotification);
    }
}