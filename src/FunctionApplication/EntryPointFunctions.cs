using System;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.FunctionApplication.Entities;
using Amolenk.ServerlessPonies.FunctionApplication.Model;
using Amolenk.ServerlessPonies.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.WebPubSub;
using Newtonsoft.Json;

namespace Amolenk.ServerlessPonies.FunctionApplication
{
    public class EntryPointFunctions
    {
        [FunctionName("Login")]
        public static WebPubSubConnection Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "login/{userId}")] HttpRequest req,
            [WebPubSubConnection(Hub = "ponies", UserId = "{userId}")] WebPubSubConnection connection)
            => connection;

        [FunctionName("Join")]
        public static async Task JoinGameSession(
            [WebPubSubTrigger("ponies", WebPubSubEventType.User, "join")]
            ConnectionContext context,
            BinaryData message,
            MessageDataType dataType,
            [DurableClient] IDurableEntityClient client,
            [WebPubSub(Hub = "ponies")] IAsyncCollector<WebPubSubOperation> operations)
        {
            var command = JsonConvert.DeserializeObject<JoinGameSessionCommand>(message.ToString());
            var gameSessionId = command.GameSessionId;

            await operations.AddAsync(new AddUserToGroup
            {
                UserId = context.UserId,
                Group = gameSessionId
            });

            var entityId = new EntityId(nameof(GameSession), gameSessionId);
            await client.SignalEntityAsync<IGameSession>(entityId, proxy => proxy.JoinAsync(context.UserId));
        }

        [FunctionName("Start")]
        public static async Task StartGameSession(
            [WebPubSubTrigger("ponies", WebPubSubEventType.User, "start")]
            ConnectionContext context,
            BinaryData message,
            MessageDataType dataType,
            [DurableClient] IDurableEntityClient client)
        {
            var command = JsonConvert.DeserializeObject<StartGameSessionCommand>(message.ToString());

            var entityId = new EntityId(nameof(GameSession), command.GameSessionId);
            await client.SignalEntityAsync<IGameSession>(entityId, proxy => proxy.StartAsync());
        }

        [FunctionName("Ping")]
        public static void Ping(
            [WebPubSubTrigger("ponies", WebPubSubEventType.User, "ping")]
            ConnectionContext context,
            BinaryData message,
            MessageDataType dataType)
        {
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

            var entityId = new EntityId(nameof(GameSession), gameName);
            return client.SignalEntityAsync<IGameSession>(entityId, proxy => proxy.PurchaseAnimalAsync(transfer));
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

            var entityId = new EntityId(nameof(GameSession), gameName);
            return client.SignalEntityAsync<IGameSession>(entityId, proxy => proxy.MoveAnimalAsync(movement));
        }

        [FunctionName("FeedAnimal")]
        public static Task FeedAnimal(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "game/{gameName}/feed-animal")] FeedAnimalCommand command,
            [DurableClient] IDurableEntityClient client,
            string gameName)
        {
            var entityId = new EntityId(nameof(AnimalBehavior), $"{gameName}:{command.AnimalName}:{command.PlayerName}");
            return client.SignalEntityAsync<IAnimalBehavior>(entityId, proxy => proxy.Feed());
        }

        [FunctionName("WaterAnimal")]
        public static Task WaterAnimal(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "game/{gameName}/water-animal")] WaterAnimalCommand command,
            [DurableClient] IDurableEntityClient client,
            string gameName)
        {
            var entityId = new EntityId(nameof(AnimalBehavior), $"{gameName}:{command.AnimalName}:{command.PlayerName}");
            return client.SignalEntityAsync<IAnimalBehavior>(entityId, proxy => proxy.Water());
        }

        [FunctionName("CleanAnimal")]
        public static Task CleanAnimal(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "game/{gameName}/clean-animal")] CleanAnimalCommand command,
            [DurableClient] IDurableEntityClient client,
            string gameName)
        {
            var entityId = new EntityId(nameof(AnimalBehavior), $"{gameName}:{command.AnimalName}:{command.PlayerName}");
            return client.SignalEntityAsync<IAnimalBehavior>(entityId, proxy => proxy.Clean());
        }
    }
}
