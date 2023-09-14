using Model;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Utils.injection;
using Utils.signal;

namespace View
{
    public class DisplayCard : InjectableBehaviour
    {
        [Inject] private NotificationsTemp _notifications;
        [Inject] private ConfigService _config;

        
        [SerializeField] private Image image;

        private int _siblingIndex;

        public readonly Signal<CardInPlay> ClickCallback = new();

        public CardInPlay Data { get; set; }
        private bool _disabled;


        private void Start()
        {
            _notifications.Add(OnNotification);
        }

        private void OnNotification(NotificationType type, bool player, string p2)
        {
            if (type == NotificationType.CardUsed && player)
                _disabled = true;

            if (type == NotificationType.Reset)
                _disabled = false;
        }

        public void OnHover()
        {
            transform.localScale = Vector3.one * 1.3f;
            transform.SetSiblingIndex(transform.parent.childCount);
        }

        public void OnOut()
        {
            transform.localScale = Vector3.one;
            transform.SetSiblingIndex(_siblingIndex);
        }

        public void OnClick()
        {
            if (_disabled)
                return;

            ClickCallback.Dispatch(Data);
        }

        public void SetData(CardInPlay value, int siblingIndex = 0)
        {
            _siblingIndex = siblingIndex;

            var texture = Resources.Load<Sprite>($"Cards/{_config.GetConfig(value.ID).Name}");
            if (texture != null)
            {
                image.sprite = texture;
            }

            Data = value;
        }

        void OnDestroy()
        {
            ClickCallback.Clear();
            _notifications.Remove(OnNotification);
        }
    }
}