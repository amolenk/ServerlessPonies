using Amolenk.ServerlessPonies.Messages;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ClientApplication
{
    public class ApiClient
    {
        private const string BaseUrl = "https://serverlessponies.azurewebsites.net/api/";
//        private const string BaseUrl = "http://localhost:7071/api/";

        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public Task JoinGame(string gameName, string playerName)
        {
            var command = new JoinGameCommand
            {
                PlayerName = playerName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"{BaseUrl}game/{gameName}/join", command);
        }

        public Task StartSinglePlayerGame(string gameName, string playerName)
        {
            var command = new StartSinglePlayerGameCommand
            {
                PlayerName = playerName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"{BaseUrl}/game/{gameName}/start", command);
        }

        public Task StartGame(string gameName)
        {
            return _client.PatchAsync($"{BaseUrl}game/{gameName}/start", null);
        }

        public Task PurchaseAnimalAsync(string gameName, string animalName, string ownerName)
        {
            var command = new PurchaseAnimalCommand
            {
                AnimalName = animalName,
                OwnerName = ownerName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"{BaseUrl}game/{gameName}/purchase-animal", command);
        }

        public Task MoveAnimal(string gameName, string animalName, string enclosureName)
        {
            var command = new MoveAnimalCommand
            {
                AnimalName = animalName,
                EnclosureName = enclosureName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"{BaseUrl}game/{gameName}/move-animal", command);
        }

        public Task FeedAnimal(string gameName, string playerName, string animalName)
        {
            var command = new FeedAnimalCommand
            {
                PlayerName = playerName,
                AnimalName = animalName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"{BaseUrl}game/{gameName}/feed-animal", command);
        }

        public Task WaterAnimal(string gameName, string playerName, string animalName)
        {
            var command = new WaterAnimalCommand
            {
                PlayerName = playerName,
                AnimalName = animalName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"{BaseUrl}game/{gameName}/water-animal", command);
        }

        public Task CleanAnimal(string gameName, string playerName, string animalName)
        {
            var command = new CleanAnimalCommand
            {
                PlayerName = playerName,
                AnimalName = animalName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"{BaseUrl}game/{gameName}/clean-animal", command);
        }
    }
}