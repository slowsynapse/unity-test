using System;
using System.Collections.Generic;
using Commands;
using Model;
using UnityEngine;
using Utils.injection;

namespace View
{
    public sealed class DisplayOpponentHand : InjectableBehaviour
    {
        [Inject] private PlayerStateModel _model;
        [Inject] private OpponentAction _opponentAction;

        [SerializeField] private GameObject cardPrefab;
        private List<GameObject> _cards;

        private void Start()
        {
            _model.Updated.Add(OnUpdated);
            _opponentAction.Add(OnOpponentAction);
            OnUpdated();
        }

        private void OnUpdated()
        {
            _cards ??= new List<GameObject>();

            if (_model.Opponent == null)//data not yet loaded
                return;

            while (_cards.Count > _model.Opponent.CardsDrawn.Length)
                DestroyOne();

            while (_cards.Count < _model.Opponent.CardsDrawn.Length)
                AddCard();

            Reposition();
        }

        private void DestroyOne()
        {
            var toRemoveIdx = _cards.Count / 2;
            Destroy(_cards[toRemoveIdx]);
            _cards.RemoveAt(toRemoveIdx);
        }

        private void AddCard()
        {
            _cards.Add(Instantiate(cardPrefab, transform));
        }


        private void Reposition()
        {
            for (var i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                card.transform.SetAsLastSibling();
                var cardCount = _cards.Count / 2f - 0.5f;
                card.GetComponent<LerpTo>().SetTarget(
                    new Vector3(
                        (-cardCount + i) * 95f,
                        (Math.Abs(cardCount - i) * 2f * Math.Abs(cardCount - i) * 2f),
                        0f),
                    (-cardCount + i) * 4f);
            }
        }

        private void OnDestroy()
        
        {
            _model.Updated.Remove(OnUpdated);
            _opponentAction.Add(OnOpponentAction);
        }

        private void OnOpponentAction()
        {
            DestroyOne();
            Reposition();
        }
    }
}