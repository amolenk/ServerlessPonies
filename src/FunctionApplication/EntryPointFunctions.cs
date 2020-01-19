using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace Amolenk.ServerlessPonies.FunctionApplication
{
    public static class EntryPointFunctions
    {
        [FunctionName("StartGame")]
        public static Task StartGame(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] PlayerInfo playerInfo,
            [DurableClient] IDurableEntityClient client)
            => client.SignalEntityAsync<IPlayer>(playerInfo.Id, player => player.StartGame(playerInfo));
    }
}
