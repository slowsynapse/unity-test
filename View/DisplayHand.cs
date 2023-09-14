using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Model;
using UnityEngine;
using Utils.injection;

namespace View
{
    public sealed class DisplayHand : InjectableBehaviour
    {
        [Inject] private GameService _gameService;
        [Inject] private PlayerHandModel _model;
        [Inject] private OpponentAction _opponentAction;

        [SerializeField] private DisplayCard cardPrefab;
        private readonly List<DisplayCard> _cards = new();

        private void Start()
        {
            _model.Updated.Add(OnHandUpdated);
            // _opponentAction.Add(OnHandUpdated);
        }


        private void OnHandUpdated()
        {
            StopAllCoroutines();
            StartCoroutine(Redraw());
        }

        private DisplayCard GetCardById(int value)
        {
            return _cards?.Find(e => e.Data.InstanceId == value);
        }

        private IEnumerator Redraw()
        {
            var existingIds = _cards.Select(e => e.Data.InstanceId).ToList();

            //remove existing cards that no longer exist in the model
            foreach (var instanceId in existingIds)
            {
                if (_model.Get().Any(c => c.InstanceId == instanceId))
                    continue;
        
                RemoveCard(instanceId);
            }
        
            foreach (var card in _model.Get())
            {
                var existing = GetCardById(card.InstanceId);
        
                if (existing != null)
                    existing.SetData(card, _cards.IndexOf(existing));
                else
                    yield return AddCard(card);
            }
        
            Reposition();
        }

        private DisplayCard CreateCard()
        {
            var cardObj = Instantiate(cardPrefab, transform);
            _cards.Add(cardObj);
            return cardObj;
        }
        
        private IEnumerator AddCard(CardInPlay value)
        {
            var card = CreateCard();
            InitCard(card, value);

            yield return new WaitForSeconds(0.2f);

            Reposition();
        }

        private void RemoveCard(int instanceId)
        {
            var card = GetCardById(instanceId);
            Destroy(card.gameObject);
            _cards.Remove(card);
        }

        private void Reposition()
        {
            for (var i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                card.transform.SetAsLastSibling();
                var cardcount = _cards.Count / 2f - 0.5f;
                card.GetComponent<LerpTo>().SetTarget(
                    new Vector3(
                        (-cardcount + i) * 95f,
                        -(Math.Abs(cardcount - i) * 2.5f * Math.Abs(cardcount - i) * 2.5f),
                        0f),
                    (-cardcount + i) * -4f);
            }
        }

        void InitCard(DisplayCard card, CardInPlay value)
        {
            card.SetData(value, _cards.IndexOf(card));
            card.transform.localPosition = Vector3.up * 300;
            card.ClickCallback.Add(OnClick);
        }

        void OnClick(CardInPlay data)
        {
            _gameService.SendUserInput(ActionType.Card, data.ID);
            // StartCoroutine(ProcessTurn(int));
        }

        // IEnumerator ProcessTurn(CardData data)
        // {
        //     var playerCard = data.image;
        //     _notifications.Dispatch(NotificationType.CardUsed, true, playerCard);
        //     yield return new WaitForSeconds(1);
        //     var opponentCard = _opponentHand.Pop().image;
        //     _notifications.Dispatch(NotificationType.CardUsed, false, opponentCard);
        //     yield return new WaitForSeconds(1);
        //     _notifications.Dispatch(NotificationType.Resolve, true, null);
        //     yield return new WaitForSeconds(0.5f);
        
        
        
        //     _notifications.Dispatch(NotificationType.PerformAction, true, playerCard);
        //     yield return new WaitForSeconds(0.7f);
        //     
        //     
        //     if (playerCard == "Battle Rifle")
        //         yield return new WaitForSeconds(0.8f);
        //
        //     var opponentDamage = 2 + (int)(10 * UnityEngine.Random.value);
        //
        //     if (playerCard != "Medkit")
        //     {
        //         switch (playerCard)
        //         {
        //             case "Battle Rifle":
        //                 _notifications.Dispatch(NotificationType.VFXRanged, false, opponentDamage.ToString());
        //                 break;
        //             case "Sword of Pepe":
        //                 _notifications.Dispatch(NotificationType.VFXSlashHorisontal, false, opponentDamage.ToString());
        //                 break;
        //             default:
        //                 _notifications.Dispatch(NotificationType.VFXSlash, false, opponentDamage.ToString());
        //                 break;
        //         }
        //         
        //         _notifications.Dispatch(NotificationType.ReceiveDamage, false, opponentDamage.ToString());
        //         _opponent.SetHP(_opponent.HP - opponentDamage);
        //     }
        //     
        //     else
        //     {
        //         _model.SetHp(_model.Hp + 4);
        //     }
        //
        //     yield return new WaitForSeconds(0.5f);
        //     _notifications.Dispatch(NotificationType.PerformAction, false, opponentCard);
        //     yield return new WaitForSeconds(0.8f);
        //     
        //
        //     if (opponentCard != "Medkit")
        //     {
        //         
        //         if (opponentCard != "Battle Rifle")
        //             yield return new WaitForSeconds(0.4f);
        //         
        //         var playerDamage = 2 + (int)(6 * UnityEngine.Random.value);
        //         
        //         switch (opponentCard)
        //         {
        //             case "Battle Rifle":
        //                 _notifications.Dispatch(NotificationType.VFXRanged, true, playerDamage.ToString());
        //                 break;
        //             case "Sword of Pepe":
        //                 _notifications.Dispatch(NotificationType.VFXSlash, true, playerDamage.ToString());
        //                 break;
        //             default:
        //                 _notifications.Dispatch(NotificationType.VFXSlashHorisontal, true, playerDamage.ToString());
        //                 break;
        //         }
        //
        //
        //         _notifications.Dispatch(NotificationType.ReceiveDamage, true, playerDamage.ToString());
        //         _model.SetHp(_model.Hp - playerDamage);
        //     }
        //
        //     else
        //     {
        //         _opponent.SetHP(_opponent.HP + 4);
        //     }
        //
        //     yield return new WaitForSeconds(1);
        //     _notifications.Dispatch(NotificationType.Reset, true, null);
        // }

        private void OnDestroy()
        {
            _model.Updated.Remove(OnHandUpdated);
        }
    }
}