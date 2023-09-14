using System.Collections;
using Model;
using Piglet;
using UnityEngine;
using Utils.injection;

namespace View
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorProxy : InjectableBehaviour
    {
        [Inject] private NotificationsTemp _notifications;

        [SerializeField] private bool isPlayer;
        private Animator _animator;
        private static readonly int Ready = Animator.StringToHash("Ready");
        private static readonly int MeleeEpic = Animator.StringToHash("Melee_Epic");
        private static readonly int Melee = Animator.StringToHash("Melee");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int HitEpic = Animator.StringToHash("Hit_Epic");
        private static readonly int Block = Animator.StringToHash("Block");
        private static readonly int Heal = Animator.StringToHash("Heal");
        private static readonly int Rifle = Animator.StringToHash("Rifle");

        void Start()
        {
            _notifications.Add(OnNotification);
            _animator = GetComponent<Animator>();
        }

        private void OnNotification(NotificationType type, bool player, string value)
        {
            Debug.Log($"{type}: {value}, {isPlayer}");

            if (isPlayer != player)
                return;


            if (type == NotificationType.CardUsed)
            {
                _animator.SetTrigger(Ready);
            }

            if (type == NotificationType.PerformAction)
            {
                
                if (value == "Battle Rifle")
                {
                    _animator.SetTrigger(Rifle);
                }
                else 
                
                if (value == "Medkit")
                {
                    _animator.SetTrigger(Heal);
                }
                else if (value == "Sword of Pepe")
                    _animator.SetTrigger(MeleeEpic);
                else
                    _animator.SetTrigger(Melee);
            }

            if (type is NotificationType.ReceiveDamage)
            {
                if (int.Parse(value) > 8)
                    _animator.SetTrigger(HitEpic);
                else if (int.Parse(value) > 0)
                    _animator.SetTrigger(Hit);
                else if (int.Parse(value) > 0)
                    _animator.SetTrigger(Block);
            }
        }

        void OnDestroy()
        {
            _notifications.Remove(OnNotification);
        }
    }
}