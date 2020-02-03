using Amolenk.ServerlessPonies.Messages;
using ClientApplication.Scenes;
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

        public Task PlaceAnimal(string animalId, string enclosureId)
        {
            var command = new PlaceAnimalCommand
            {
                PlayerId = "",
                AnimalId = animalId,
                EnclosureId = enclosureId
            };

            // TODO Use better REST resources.
            return _client.SendJsonAsync(HttpMethod.Post, $"http://localhost:7071/api/PlaceAnimal", command);
        }
    }
}