using System;
using System.Collections;
using Commands;
using Model;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.injection;

namespace View
{
    [RequireComponent(typeof(Image))]
    public class DisplayCounterElement : InjectableBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Inject] private ConfigService _config;

        public Action<DisplayCounterElement, bool> PointerOverUpdated;

        public int owner = 0;

        public PlayerActionPayload Data;
        private Image _image;
        private bool _resolved;

        public DisplayCounterElement(PlayerActionPayload data)
        {
            Data = data;
        }

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        public void Resolve()
        {
            if (_resolved) return;

            StartCoroutine(Flip());
            _resolved = true;
        }

        private IEnumerator Flip()
        {
            while (transform.localScale.x > 0.01f)
            {
                transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * 8, 1, 1);
                yield return null;
            }

            Debug.Log($"Icons.Actions/{Data.Type.ToString()}");
            
            var sprite = Data.Type switch
            {
                ActionType.Card => Resources.Load<Sprite>($"Counter/{_config.GetConfig(Data.Card.ID).Name}"),
                _ => Resources.Load<Sprite>($"Icons/Actions/{Data.Type.ToString()}")
            };

            if (sprite != null)
                _image.sprite = sprite;

            while (transform.localScale.x < 1)
            {
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 8, 1, 1);
                yield return null;
            }

            transform.localScale = Vector3.one;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerOverUpdated?.Invoke(this, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerOverUpdated?.Invoke(this, false);
        }
    }
}