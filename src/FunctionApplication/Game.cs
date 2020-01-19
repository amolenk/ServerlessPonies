using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace Amolenk.ServerlessPonies.FunctionApplication
{
    public interface IGame
    {
        void Play(string playerId);
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Game : IGame
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Game(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [JsonProperty]
        public string PlayerId { get; set; }

        public void Play(string playerId)
        {
            this.PlayerId = playerId;

            Entity.Current.SignalEntity<IPlayer>(PlayerId, player => player.NotifyPlayer(new
            { 
                foo = "bar"
            }));
        } 

        [FunctionName(nameof(Game))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<Game>();
    }
}