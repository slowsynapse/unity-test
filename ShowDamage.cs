using Model;
using UnityEngine;
using Utils.injection;
using view.behaviours.UI;

public class ShowDamage : InjectableBehaviour
{
    private MoveUpAndFade _renderer;

    public NotificationType notification;

    [Inject] private NotificationsTemp _notifications;

    [SerializeField] private bool isPlayer;

    private void Start()
    {
        _renderer = GetComponent<MoveUpAndFade>();

        _notifications.Add(OnNotification);
    }

    private void OnNotification(NotificationType type, bool player, string value)
    {
        if (isPlayer == player && notification == type)
        {
            _renderer.SetValue(value);
        }
    }


    void OnDestroy()
    {
        _notifications.Remove(OnNotification);
    }
}