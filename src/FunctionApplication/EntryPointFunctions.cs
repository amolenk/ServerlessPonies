using System.Threading.Tasks;
using Amolenk.ServerlessPonies.FunctionApplication.Entities;
using Amolenk.ServerlessPonies.FunctionApplication.Model;
using Amolenk.ServerlessPonies.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace Amolenk.ServerlessPonies.FunctionApplication
{
    public class EntryPointFunctions
    {
        // When the client app opens, it requires valid connection credentials to connect to the
        // Azure SignalR service. This function will return the connection information.
        [FunctionName("GetSignalRInfo")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "signalr/{userId}/negotiate")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "ponies", UserId = "{userId}")] SignalRConnectionInfo connectionInfo)
            => connectionInfo;

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

            var entityId = new EntityId(nameof(GameSession), gameName);
            await client.SignalEntityAsync<IGameSession>(entityId, proxy => proxy.StartSinglePlayer(command.PlayerName));
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
