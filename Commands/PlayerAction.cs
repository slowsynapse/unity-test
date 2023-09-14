using Model;
using Newtonsoft.Json;
using Utils.injection;
using Utils.signal;

namespace Commands
{
    public class PlayerActionPayload
    {
        [JsonProperty("type")] public ActionType Type;
        [JsonProperty("card")] public CardInPlay Card;
    }

    [Singleton]
    public class PlayerAction : Signal<PlayerActionPayload>
    {
    }
}