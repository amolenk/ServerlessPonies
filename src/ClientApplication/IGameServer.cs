using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Amolenk.ServerlessPonies.ClientApplication
{
    public interface IGameServer
    {
        Task LoginAsync(string playerName, Action<JObject> messageHandler);

        Task JoinAsync(string gameSessionId);

        Task StartAsync(string gameSessionId);

        Task SendCustomEventAsync<T>(string eventName, T eventData);
    }
}