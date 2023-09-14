using Model;
using Newtonsoft.Json;
using Utils.injection;
using Utils.signal;

namespace Commands
{
    [Singleton]
    public class Resolve : Signal<PlayerActionPayload, PlayerActionPayload>
    {
    }
}