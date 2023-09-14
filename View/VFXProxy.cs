using System.Collections;
using Model;
using UnityEngine;
using Utils.injection;

public class VFXProxy : InjectableBehaviour
{
    [Inject] private NotificationsTemp _notifications;

    [SerializeField] private bool isPlayer;

    [SerializeField] private GameObject heal;
    [SerializeField] private GameObject impact;
    [SerializeField] private GameObject impactRanged;
    [SerializeField] private GameObject impactHorisontal;
    [SerializeField] private GameObject block;

    void Start()
    {
        _notifications.Add(OnNotification);
    }

    private void OnNotification(NotificationType type, bool player, string value)
    {
        if (isPlayer != player)
            return;

        if (type == NotificationType.PerformAction)
        {
            StartCoroutine(ProcessAction(value));
        }

        else if (type == NotificationType.VFXSlash)
            foreach (var sys in impact.GetComponentsInChildren<ParticleSystem>())
                sys.Play();
        
        else if (type == NotificationType.VFXSlashHorisontal)
            foreach (var sys in impactHorisontal.GetComponentsInChildren<ParticleSystem>())
                sys.Play();

        else if (type == NotificationType.VFXRanged)
            foreach (var sys in impactRanged.GetComponentsInChildren<ParticleSystem>())
                sys.Play();
    }

    private IEnumerator ProcessAction(string value)
    {
        yield return new WaitForSeconds(1);
        if (value == "Medkit")
        {
            foreach (var sys in heal.GetComponentsInChildren<ParticleSystem>())
                sys.Play();
        }
    }

    void OnDestroy()
    {
        _notifications.Remove(OnNotification);
    }
}