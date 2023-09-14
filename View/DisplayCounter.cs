using System.Collections.Generic;
using Commands;
using Model;
using UnityEngine;
using Utils.injection;

namespace View
{
    public class DisplayCounter : InjectableBehaviour
    {
        public DisplayCounterElement cardBack;
        public DisplayCard cardDetails;

        [Inject] private PlayerAction _playerAction;
        [Inject] private OpponentAction _opponentAction;
        [Inject] private Resolve _resolve;
        [Inject] private TurnReset _turnReset;
        [Inject] private PlayerStateModel _model;

        private readonly List<DisplayCounterElement> _elements = new();

        private const float ReferenceHeight = 900;

        void Start()
        {
            _playerAction.Add(OnPlayerAction);
            _opponentAction.Add(OnOpponentAction);
            _resolve.Add(OnResolve);
            _model.Updated.Add(OnModelUpdated);
        }


        private void OnPlayerAction(PlayerActionPayload value)
        {
            var elem = CreateCounterElement(new Vector3(0, 0, 0),
                Vector3.left * (40 + Random.value * 20) + Vector3.up * ReferenceHeight / 2, 0);
            elem.Data = value;
        }

        private DisplayCounterElement CreateCounterElement(Vector3 from, Vector3 to, int owner)
        {
            var elem = Instantiate(cardBack, transform);
            elem.transform.localPosition = from;
            elem.GetComponent<LerpTo>().CurrentRotation = (int)(Random.value * 5 + 5) * (Random.value < 0.5 ? 1 : -1);

            elem.GetComponent<LerpTo>().SetTarget(
                to,
                (int)(Random.value * 3 + 3) * (Random.value < 0.5 ? 1 : -1));
            elem.owner = owner;
            _elements.Add(elem);
            return elem;
        }


        private void OnOpponentAction()
        {
            var elem = CreateCounterElement(new Vector3(0, ReferenceHeight, 0),
                Vector3.right * (40 + Random.value * 20) + Vector3.up * ReferenceHeight / 2, 1);
        }

        private void OnResolve(PlayerActionPayload playerAction, PlayerActionPayload opponentAction)
        {
            foreach (var elem in _elements)
            {
                elem.Resolve();
                elem.Data ??= opponentAction;
                elem.GetComponent<LerpTo>().ResetRotation();
            }
        }

        private void OnModelUpdated()
        {
            var playerCards = 0;
            var opponentCards = 0;
            for (var i = _elements.Count - 1; i >= 0; i--)
            {
                var elem = _elements[i];
                if (elem.Data.Card == null || !_model.IsCardInPlay(elem.Data.Card.InstanceId))
                {
                    _elements.Remove(elem);
                    Destroy(elem.gameObject);
                    continue;
                }

                var cardCount = (elem.owner == 0 ? playerCards : opponentCards);
                var position = (elem.owner == 0 ? Vector3.left : Vector3.right) * (735 - 110 * (cardCount % 2));
                position += Vector3.up * (115 * (cardCount / 2) + 270); //270 is portrait offset

                elem.GetComponent<LerpTo>().SetTarget(position, 0);

                if (elem.PointerOverUpdated == null)
                    elem.PointerOverUpdated += ShowCounterDetails;

                if (elem.owner == 0)
                    playerCards++;
                else opponentCards++;
            }
        }

        private void ShowCounterDetails(DisplayCounterElement elem, bool pointerOver)
        {
            if (elem.Data.Card == null)
                return;
            cardDetails.SetData(elem.Data.Card);

            var anchoredPosition = elem.GetComponent<RectTransform>().anchoredPosition;
            cardDetails.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(anchoredPosition.x > 0 ? 680 : -680, anchoredPosition.y + 50);
            cardDetails.gameObject.SetActive(pointerOver);
        }

        void OnDestroy()
        {
            _playerAction.Remove(OnPlayerAction);
            _opponentAction.Remove(OnOpponentAction);
            _resolve.Remove(OnResolve);
            _model.Updated.Add(OnModelUpdated);

            cardDetails.gameObject.SetActive(false);
        }
    }
}