using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.FunctionApplication.Model;
using Amolenk.ServerlessPonies.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Extensions.WebPubSub;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameSession : IGameSession
    {
        private readonly IAsyncCollector<WebPubSubOperation> _operations;

        public GameSession(IAsyncCollector<WebPubSubOperation> operations)
        {
            _operations = operations;

            this.PlayerStates = new PlayerStateCollection();
            this.AnimalStates = new AnimalStateCollection();
        }

        [JsonProperty]
        public bool IsStarted { get; set; }

        [JsonProperty]
        public PlayerStateCollection PlayerStates { get; set; }

        [JsonProperty]
        public AnimalStateCollection AnimalStates { get; set; }

        public Task JoinAsync(string playerName)
        {
            AddPlayer(playerName);

            return PublishEventAsync(new PlayerJoinedEvent
            {
                GameSessionId = Entity.Current.EntityKey,
                Players = PlayerStates
                    .Select(player => new Messages.PlayerState
                    {
                        Name = player.Name,
                        Credits = player.Credits
                    })
                    .ToList()
            });
        }

        public Task StartAsync()
        {
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

        public Task DepositCreditsAsync(CreditsDeposit deposit)
        {
            var state = PlayerStates[deposit.PlayerName];
            state.Credits += deposit.Amount;

            return PublishEventAsync(new CreditsChangedEvent
            {
                PlayerName = deposit.PlayerName,
                Credits = state.Credits
            });
        }

        private void AddPlayer(string playerName)
        {
            var playerState = Model.PlayerState.Default(playerName);

            PlayerStates.Add(playerState);
        }

        private Task PublishEventAsync<T>(T message)
        {
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            var envelope = new EventEnvelope
            {
                EventType = typeof(T).Name,
                EventBody = message
            };

            return _operations.AddAsync(new SendToGroup
            {
                Group = Entity.Current.EntityKey,
                DataType = MessageDataType.Json,
                Message = BinaryData.FromObjectAsJson(envelope, jsonOptions)
            });
        }

        [FunctionName(nameof(GameSession))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx,
            [WebPubSub(Hub = "ponies")] IAsyncCollector<WebPubSubOperation> operations)
            => ctx.DispatchAsync<GameSession>(operations);
    }
}