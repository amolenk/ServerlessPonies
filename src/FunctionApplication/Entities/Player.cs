using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{


    [JsonObject(MemberSerialization.OptIn)]
    public class Player : IPlayer
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Player(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [JsonProperty]
        public PlayerInfo PlayerInfo { get; set; }

        public void StartGame(PlayerInfo playerInfo)
        {
            PlayerInfo = playerInfo;

            Entity.Current.SignalEntity<IGame>("OneAndOnlyGame", game => game.Play(playerInfo.Id));
        }

        public async Task NotifyPlayer(object eventPayload)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            await httpClient.PostAsJsonAsync($"http://localhost:7071/api/events/{PlayerInfo.Id}", eventPayload);
        }

        [FunctionName(nameof(Player))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<Player>();

        // if (!ctx.HasState)
        // ctx.SetState(...);
    }
}