using Model;
using Newtonsoft.Json;
using Utils.injection;
using Utils.signal;

namespace Commands
{
    [Singleton]
    public class TurnReset : Signal<int>
    {
    }
}