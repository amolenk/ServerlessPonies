using Amolenk.ServerlessPonies.Messages;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace Amolenk.ServerlessPonies.ClientApplication
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(HttpClient client, IConfiguration configuration, ILogger<ApiClient> logger)
        {
            _client = client;
            _client.BaseAddress = new Uri(configuration.GetValue<string>("FunctionsBaseUrl"));
            _logger = logger;
        }

        public async Task<Uri> LoginAsync(string playerName)
        {
            var response = await _client.GetAsync($"/api/login/{playerName}");
            var result = await response.Content.ReadAsStringAsync();
            var connectionInfo = JObject.Parse(result);

            _logger.LogInformation("Logged in!");

            return new Uri(connectionInfo["url"].Value<string>());
        }

        public Task StartSinglePlayerGameAsync(string gameName, string playerName)
        {
            var command = new StartSinglePlayerGameCommand
            {
                PlayerName = playerName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/start", command);
        }

        public Task PurchaseAnimalAsync(string gameName, string animalName, string ownerName)
        {
            var command = new PurchaseAnimalCommand
            {
                AnimalName = animalName,
                OwnerName = ownerName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/purchase-animal", command);
        }

        public Task MoveAnimalAsync(string gameName, string animalName, string enclosureName)
        {
            var command = new MoveAnimalCommand
            {
                AnimalName = animalName,
                EnclosureName = enclosureName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/move-animal", command);
        }

        public Task FeedAnimalAsync(string gameName, string playerName, string animalName)
        {
            var command = new FeedAnimalCommand
            {
                PlayerName = playerName,
                AnimalName = animalName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/feed-animal", command);
        }

        public Task WaterAnimalAsync(string gameName, string playerName, string animalName)
        {
            var command = new WaterAnimalCommand
            {
                PlayerName = playerName,
                AnimalName = animalName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/water-animal", command);
        }

        public Task CleanAnimalAsync(string gameName, string playerName, string animalName)
        {
            var command = new CleanAnimalCommand
            {
                PlayerName = playerName,
                AnimalName = animalName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/clean-animal", command);
        }
    }
}