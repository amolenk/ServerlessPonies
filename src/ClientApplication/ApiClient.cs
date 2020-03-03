using Amolenk.ServerlessPonies.Messages;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace ClientApplication
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _client.BaseAddress = new Uri(configuration.GetValue<string>("FunctionsBaseUrl"));
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