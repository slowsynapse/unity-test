using System.Collections.Generic;
using System.Linq;
using Piglet.Newtonsoft.Json;
using Utils.injection;
using Utils.signal;

namespace Model
{
    public class CardInPlay
    {
        [JsonProperty("id")] public int ID;
        [JsonProperty("instanceId")] public int InstanceId;

        public override bool Equals(object other)
        {
            if (other == null) return false;
            return other is CardInPlay otherCard && Equals(otherCard);
        }

        private bool Equals(CardInPlay other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }

    public class User
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("portraitSrc")] public string PortraitSrc;
    }

    public class PlayerGameState
    {
        [JsonProperty("cardsInPlay")] public CardInPlay[] CardsInPlay;
        [JsonProperty("cardsDrawn")] public int[] CardsDrawn;
        [JsonProperty("equipmentSlots")] public Dictionary<int, CardInPlay> EquipmentSlots;

        [JsonProperty("health")] public int Health;
        [JsonProperty("willpower")] public int Willpower;

        [JsonProperty("user")] public User User;
    }

    [Singleton]
    public class PlayerStateModel
    {
        public PlayerGameState Player => _data.ContainsKey(PlayerId) ? _data[PlayerId] : null;
        public PlayerGameState Opponent => _data.ContainsKey(OpponentId) ? _data[OpponentId] : null;

        public Signal Updated { get; } = new();

        private Dictionary<string, PlayerGameState> _data = new();

        public string PlayerId = "default";
        public string OpponentId = "bot_level1";

        public void Set(Dictionary<string, PlayerGameState> value)
        {
            _data = value;
            Updated.Dispatch();
        }

        public bool IsCardInPlay(int value)
        {
            return _data.Values.Any(
                data =>
                    data.CardsInPlay.Any(
                        c =>
                            c.InstanceId == value));
        }
    }
}