using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.FunctionApplication.Entities;
using Amolenk.ServerlessPonies.FunctionApplication.Model;
using Amolenk.ServerlessPonies.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Amolenk.ServerlessPonies.FunctionApplication
{
    public class EntryPointFunctions
    {
        private readonly IEventPublisher _eventPublisher;

        public EntryPointFunctions(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        [FunctionName("JoinGame")]
        public static Task JoinGame(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "game/{gameName}/join")] JoinGameCommand command,
            [DurableClient] IDurableEntityClient client,
            string gameName)
        {
            var entityId = new EntityId(nameof(Game), gameName);
            return client.SignalEntityAsync<IGame>(entityId, proxy => proxy.Join(command.PlayerName));
        }

        [FunctionName("StartGame")]
        public static Task StartGame(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PATCH", Route = "game/{gameName}/start")] HttpRequest request,
            [DurableClient] IDurableEntityClient client,
            string gameName)
        {
            var entityId = new EntityId(nameof(Game), gameName);
            return client.SignalEntityAsync<IGame>(entityId, proxy => proxy.Start());
        }
        
        [FunctionName("StartSinglePlayerGame")]
        public static async Task StartSinglePlayerGame(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "game/{gameName}/start")] StartSinglePlayerGameCommand command,
            [DurableClient] IDurableEntityClient client,
            [SignalR(HubName = "ponies")] IAsyncCollector<SignalRGroupAction> signalRGroupActions,
            string gameName)
        {
            await signalRGroupActions.AddAsync(new SignalRGroupAction
            {
                UserId = command.PlayerName,
                GroupName = gameName,
                Action = GroupAction.Add
            });

            var entityId = new EntityId(nameof(Game), gameName);
            await client.SignalEntityAsync<IGame>(entityId, proxy => proxy.StartSinglePlayer(command.PlayerName));
        }

        [FunctionName("Ping")]
        public static void Ping(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Received PING request.");
        }

        [FunctionName("PurchaseAnimal")]
        public static Task PurchaseAnimal(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "game/{gameName}/purchase-animal")] PurchaseAnimalCommand command,
            [DurableClient] IDurableEntityClient client,
            string gameName)
        {
            var transfer = new AnimalPurchase
            {
                AnimalName = command.AnimalName,
                NewOwnerName = command.OwnerName
            };

            var entityId = new EntityId(nameof(Game), gameName);
            return client.SignalEntityAsync<IGame>(entityId, proxy => proxy.PurchaseAnimalAsync(transfer));
        }

        [FunctionName("MoveAnimal")]
        public static Task MoveAnimal(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "game/{gameName}/move-animal")] MoveAnimalCommand command,
            [DurableClient] IDurableEntityClient client,
            string gameName)
        {
            var movement = new AnimalMovement
            {
                AnimalName = command.AnimalName,
                NewEnclosureName = command.EnclosureName
            };

            var entityId = new EntityId(nameof(Game), gameName);
            return client.SignalEntityAsync<IGame>(entityId, proxy => proxy.MoveAnimalAsync(movement));
        }

        // [FunctionName("AnimalState")]
        // public static async Task<HttpResponseMessage> Run(
        //     [HttpTrigger(AuthorizationLevel.Function)] HttpRequestMessage req,
        //     [DurableClient] IDurableEntityClient client)
        // {
        //     var entityId = new EntityId(nameof(Animal), "foo");
        //     EntityStateResponse<JObject> stateResponse = await client.ReadEntityStateAsync<JObject>(entityId);
        //     return req.CreateResponse(HttpStatusCode.OK, stateResponse.EntityState);
        // }
    }
}
