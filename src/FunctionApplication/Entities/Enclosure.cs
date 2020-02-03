using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Enclosure : IEnclosure
    {
        private readonly IDurableEntityContext _context;

        public Enclosure(IDurableEntityContext context)
        {
            _context = context;
        }

        [JsonProperty]
        public string OwnerId { get; set; }

        [JsonProperty]
        public string AnimalId { get; set; }

        public Task<string> GetOwnerId()
            => Task.FromResult(OwnerId);

        public Task PlaceAnimal(string animalId)
        {
            // TODO Check that there's no other animal yet.
            // Or; in orchestrator?

//            Entity.Current.SignalEntity<IAnimal>(animalId, animal => animal.EnableActivity());

            AnimalId = animalId;

            return Task.CompletedTask;
        }

        [FunctionName(nameof(Enclosure))]
        public static Task Run([EntityTrigger] IDurableEntityContext context)
            => context.DispatchAsync<Enclosure>(context);
    }
}