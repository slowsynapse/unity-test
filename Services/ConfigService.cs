using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using utils;
using Utils.injection;

namespace Services
{
    public class CardConfig
    {
        [JsonProperty("name")] public string Name;
    }

    [Singleton]
    public class ConfigService
    {
        private CardConfig[] _config;
        public bool IsLoaded => _config != null;

        public async void Init()
        {
            var request = UnityWebRequest.Get("https://api.enter-the-sphere.com:5000/api/v1/cards");
            await request.SendWebRequest();
            _config = JsonConvert.DeserializeObject<CardConfig[]>(request.downloadHandler.text);
        }

        public CardConfig GetConfig(int index)
        {
            return _config[index];
        }
    }
}