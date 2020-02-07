using System.Linq;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.FunctionApplication.Model;
using Amolenk.ServerlessPonies.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Game : IGame
    {
        private readonly IAsyncCollector<SignalRMessage> _signalRMessages;

        public Game(IAsyncCollector<SignalRMessage> signalRMessages)
        {
            _signalRMessages = signalRMessages;

            this.PlayerStates = new PlayerStateCollection();
            this.AnimalStates = new AnimalStateCollection();
        }

        [JsonProperty]
        public bool IsStarted { get; set; }

        [JsonProperty]
        public PlayerStateCollection PlayerStates { get; set; }

        [JsonProperty]
        public AnimalStateCollection AnimalStates { get; set; }

        public void Join(string playerName)
        {
            if (!IsStarted)
            {
                AddPlayer(playerName);
            }

            // TODO What else?
        }

        public Task Start()
        {
            IsStarted = true;

            return PublishEventAsync(new GameStartedEvent
            {
                GameName = Entity.Current.EntityName
            });
        }

        public Task StartSinglePlayer(string playerName)
        {
            AddPlayer(playerName);

            IsStarted = true;
            AnimalStates = AnimalStateCollection.InitialGameState();

            return PublishEventAsync(new GameStartedEvent
            {
                GameName = Entity.Current.EntityKey,
                Animals = AnimalStates
                    .Select(animal => new Messages.AnimalState
                    {
                        Name = animal.Name,
                        Price = animal.Price,
                        OwnerName = animal.OwnerName,
                        EnclosureName = animal.EnclosureName
                    })
                    .ToList()
            });
        }

        public Task PurchaseAnimalAsync(AnimalPurchase purchase)
        {
            // TODO Check for enough credits.

            AnimalStates[purchase.AnimalName].OwnerName = purchase.NewOwnerName;

            return PublishEventAsync(new AnimalPurchasedEvent
            {
                AnimalName = purchase.AnimalName,
                OwnerName = purchase.NewOwnerName
            });
        }

        public Task MoveAnimalAsync(AnimalMovement movement)
        {
            AnimalStates[movement.AnimalName].EnclosureName = movement.NewEnclosureName;

            return PublishEventAsync(new AnimalMovedEvent
            {
                AnimalName = movement.AnimalName,
                EnclosureName = movement.NewEnclosureName
            });
        }

        private void AddPlayer(string playerName)
        {
            var playerState = PlayerState.Default(playerName);

            PlayerStates.Add(playerState);
        }

        private Task PublishEventAsync<T>(T @event)
        {
            return _signalRMessages.AddAsync(new SignalRMessage
                {
                    GroupName = Entity.Current.EntityKey,
                    Target = "handleEvent",
                    Arguments = new object[] { typeof(T).Name, @event }
                });
        }

        [FunctionName(nameof(Game))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx,
            [SignalR(HubName = "ponies")] IAsyncCollector<SignalRMessage> signalRMessages)
            => ctx.DispatchAsync<Game>(signalRMessages);
    }
}