using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Utils.injection;
using View;

public class CameraShake : InjectableBehaviour
{
    [Inject] private NotificationsTemp _notifications;

    private LerpTo _lerp;

    private void Start()
    {
        _lerp = GetComponent<LerpTo>();

        _notifications.Add(OnNotification);
    }

    private void OnNotification(NotificationType type, bool player, string value)
    {
        if (type is NotificationType.ReceiveDamage && int.Parse(value)>0)
        {
            StopAllCoroutines();
            StartCoroutine(MoveTo(Vector3.left + Vector3.forward, 15));
        }
    }

    public IEnumerator MoveTo(Vector3 position, float rotation)
    {
        {
            _lerp.SetTarget(position, rotation, true);
            yield return new WaitForSeconds(1f);
            _lerp.SetTarget(Vector3.zero, 0, true);
        }


    }
    
    void OnDestroy()
    {
        _notifications.Remove(OnNotification);
    }
}
