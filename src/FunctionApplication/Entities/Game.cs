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
                Players = PlayerStates
                    .Select(player => new Messages.PlayerState
                    {
                        Name = player.Name,
                        Credits = player.Credits
                    })
                    .ToList(),
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

        public async Task PurchaseAnimalAsync(AnimalPurchase purchase)
        {
            var owner = PlayerStates[purchase.NewOwnerName];
            var animal = AnimalStates[purchase.AnimalName];

            if (animal?.OwnerName == null && animal?.Price <= owner?.Credits)
            {
                owner.Credits -= animal.Price;
                animal.OwnerName = owner.Name;
            
                await PublishEventAsync(new AnimalPurchasedEvent
                {
                    AnimalName = purchase.AnimalName,
                    OwnerName = purchase.NewOwnerName
                });

                await PublishEventAsync(new CreditsChangedEvent
                {
                    PlayerName = owner.Name,
                    Credits = owner.Credits
                });

                // Key of the animal behavior entity is:
                // <GameName>:<AnimalName>:<OwnerName>
                var entityKey = $"{Entity.Current.EntityKey}:{purchase.AnimalName}:{purchase.NewOwnerName}";
                var entityId = new EntityId(nameof(AnimalBehavior), entityKey);
                Entity.Current.SignalEntity<IAnimalBehavior>(entityId, proxy => proxy.Start());
            }
            else
            {
                await PublishEventAsync(new AnimalPurchaseFailedEvent
                {
                    AnimalName = purchase.AnimalName,
                    OwnerName = purchase.NewOwnerName
                });
            }
        }

        public async Task MoveAnimalAsync(AnimalMovement movement)
        {
            var animalState = AnimalStates[movement.AnimalName];
            if (animalState.EnclosureName != movement.NewEnclosureName)
            {
                animalState.EnclosureName = movement.NewEnclosureName;

                await PublishEventAsync(new AnimalMovedEvent
                {
                    AnimalName = movement.AnimalName,
                    EnclosureName = movement.NewEnclosureName
                });
            }
        }

        public Task UpdateAnimalMoodAsync(AnimalMoodChange mood)
        {
            var state = AnimalStates[mood.AnimalName];
            state.HappinessLevel = mood.HappinessLevel;
            state.HungrinessLevel = mood.HungrinessLevel;
            state.ThirstinessLevel = mood.ThirstinessLevel;

            return PublishEventAsync(new AnimalMoodChangedEvent
            {
                AnimalName = state.Name,
                HappinessLevel = state.HappinessLevel,
                HungrinessLevel = state.HungrinessLevel,
                ThirstinessLevel = state.ThirstinessLevel
            });
        }

        private void AddPlayer(string playerName)
        {
            var playerState = Model.PlayerState.Default(playerName);

            PlayerStates.Add(playerState);
        }

        private Task PublishEventAsync<T>(T @event)
        {
            return _signalRMessages.AddAsync(new SignalRMessage
                {
                    GroupName = Entity.Current.EntityKey,
                    Target = "HandleMessage",
                    Arguments = new object[] { typeof(T).Name, @event }
                });
        }

        [FunctionName(nameof(Game))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx,
            [SignalR(HubName = "ponies")] IAsyncCollector<SignalRMessage> signalRMessages)
            => ctx.DispatchAsync<Game>(signalRMessages);
    }
}