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

            return _client.SendJsonAsync(HttpMethod.Post, $"http://localhost:7071/api/game/{gameName}/join", command);
        }

        public Task StartSinglePlayerGame(string gameName, string playerName)
        {
            var command = new StartSinglePlayerGameCommand
            {
                PlayerName = playerName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"http://localhost:7071/api/game/{gameName}/start", command);
        }

        public Task StartGame(string gameName)
        {
            return _client.PatchAsync($"http://localhost:7071/api/game/{gameName}/start", null);
        }

        public Task PurchaseAnimalAsync(string gameName, string animalName, string ownerName)
        {
            var command = new PurchaseAnimalCommand
            {
                AnimalName = animalName,
                OwnerName = ownerName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"http://localhost:7071/api/game/{gameName}/purchase-animal", command);
        }

        public Task MoveAnimal(string gameName, string animalName, string enclosureName)
        {
            var command = new MoveAnimalCommand
            {
                AnimalName = animalName,
                EnclosureName = enclosureName
            };

            return _client.SendJsonAsync(HttpMethod.Post, $"http://localhost:7071/api/game/{gameName}/move-animal", command);
        }
    }
}