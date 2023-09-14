using System.Collections;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Utils.injection;

[RequireComponent(typeof(Image))]
public class MotionGraphics : InjectableBehaviour
{
    public Sprite[] MeleeHitAnimation;
    public Sprite[] RangedHitAnimation;
    private Image _renderer;

    public NotificationType notification;

    [Inject] private NotificationsTemp _notifications;

    private void Start()
    {
        _renderer = GetComponent<Image>();
        _renderer.enabled = false;

        // _notifications.Add(OnNotification);
    }

    private void OnNotification(NotificationType type, string value)
    {
        if (type == notification)
        {
            StopAllCoroutines();
            StartCoroutine(PlayAnim(Random.value < 0.5 ? MeleeHitAnimation : RangedHitAnimation));
        }
    }

    private IEnumerator PlayAnim(Sprite[] value)
    {
        _renderer.enabled = true;
        foreach (var sprite in value)
        {
            _renderer.sprite = sprite;
            yield return new WaitForSeconds(0.03f);
        }

        _renderer.enabled = false;
    }

    void OnDestroy()
    {
        // _notifications.Remove(OnNotification);
    }
}