using System.Collections.Generic;
using System.Linq;
using Commands;
using Utils.injection;
using Utils.signal;

namespace Model
{
    [Singleton]
    public class PlayerHandModel
    {
        [Inject] private DrawCard _drawCard;
        [Inject] private PlayerAction _playerAction;
        
        private List<CardInPlay> _data = new();
    
        public Signal Updated { get; } = new();

        public PlayerHandModel()
        {
            Injector.Instance.Resolve(this);
            _drawCard.Add(AddCard);
            _playerAction.Add(OnAction);
        }
        
    
        public IEnumerable<CardInPlay> Get()
        {
            return _data;
        }

        private void AddCard(CardInPlay value)
        {
            _data.Add(value);
            Updated.Dispatch();
        }
        
        private void OnAction(PlayerActionPayload value)
        {
            if (value.Card == null) return;
            
            _data.Remove(value.Card);
            Updated.Dispatch();
        }
    
        public void Trim(int [] cardIds)
        {
            _data = _data.Where(c => cardIds.Contains(c.ID)).ToList();
            Updated.Dispatch();
        }

        ~PlayerHandModel()
        {
            _drawCard.Remove(AddCard);
            _playerAction.Remove(OnAction);
        }
    }
}