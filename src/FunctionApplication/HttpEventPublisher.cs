using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace Amolenk.ServerlessPonies.FunctionApplication
{
    public class HttpEventPublisher : IEventPublisher
    {
        private readonly HttpClient _client;

        public HttpEventPublisher(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task PublishAsync(string playerId, object eventPayload)
        {
            return _client.PostAsJsonAsync($"http://localhost:7071/api/events/{playerId}", eventPayload);
        }
    }
}