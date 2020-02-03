using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.FunctionApplication.Dto;
using Amolenk.ServerlessPonies.FunctionApplication.Entities;
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

        [FunctionName("StartGame")]
        public static Task StartGame(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] PlayerInfo playerInfo,
            [DurableClient] IDurableEntityClient client)
            => client.SignalEntityAsync<IPlayer>(playerInfo.Id, player => player.StartGame(playerInfo));

        // [FunctionName("BuyAnimal")]
        // public static async Task BuyAnimal(
        //     [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] BuyAnimalRequestDto request,
        //     [DurableClient] IDurableOrchestrationClient client,
        //     ILogger log)
        // {
        //     string instanceId = await client.StartNewAsync("BuyAnimalCore", request);

        //     log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        // }

        // [FunctionName("BuyAnimalCore")]
        // public async Task BuyAnimalCore(
        //     [OrchestrationTrigger] IDurableOrchestrationContext context)
        // {
        //     BuyAnimalRequestDto request = context.GetInput<BuyAnimalRequestDto>();

        //     var animalEntity = new EntityId(nameof(Animal), request.AnimalId);
        //     var enclosureEntity = new EntityId(nameof(Enclosure), request.EnclosureId);

        //     using (await context.LockAsync(animalEntity, enclosureEntity))
        //     {
        //         IEnclosure enclosureProxy = context.CreateEntityProxy<IEnclosure>(enclosureEntity);
        //         var enclosureOwner = await enclosureProxy.GetOwnerId();

        //         IAnimal animalProxy = context.CreateEntityProxy<IAnimal>(animalEntity);

        //         await animalProxy.SetOwner(request.PlayerId);
        //     }

        //     await _eventPublisher.PublishAsync(request.PlayerId, MessageEnvelope.Wrap(new AnimalPlacedEvent
        //     {
        //         OwnerId = request.PlayerId,
        //         EnclosureId = "[enclosure]",
        //         AnimalId = request.AnimalId
        //     }));
        // }

        [FunctionName("PlaceAnimal")]
        public static async Task PlaceAnimal(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] PlaceAnimalCommand command,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            string instanceId = await client.StartNewAsync("PlaceAnimalCore", command);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }

        [FunctionName("PlaceAnimalCore")]
        public async Task PlaceAnimalCore(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [SignalR(HubName = "ponies")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            PlaceAnimalCommand command = context.GetInput<PlaceAnimalCommand>();

            var enclosureEntityId = new EntityId(nameof(Enclosure), command.EnclosureId);
            var animalEntityId = new EntityId(nameof(Animal), command.AnimalId);

            using (await context.LockAsync(enclosureEntityId, animalEntityId))
            {
                // TODO Verify that player owns enclosure.
                // TODO Verify that player owns animal.

                IEnclosure enclosureEntityProxy = context.CreateEntityProxy<IEnclosure>(enclosureEntityId);
                await enclosureEntityProxy.PlaceAnimal(command.AnimalId);

                IAnimal animalEntityProxy = context.CreateEntityProxy<IAnimal>(animalEntityId);
                await animalEntityProxy.SetOwner("owner");
            }

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "newMessage",// TODO Rename to 'handleEvent'
                    Arguments = new[] { MessageEnvelope.Wrap(new AnimalPlacedEvent
                    {
                        EnclosureId = command.EnclosureId,
                        AnimalId = command.AnimalId
                    }) }
                });

//            await _eventPublisher.PublishAsync(command.PlayerId, MessageEnvelope.Wrap());
        }
    }
}
