using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using Amolenk.ServerlessPonies.Messages;
using Newtonsoft.Json;
using System.Threading;
using System.Text;

namespace Amolenk.ServerlessPonies.ClientApplication
{
    public sealed class GameServer : IGameServer
    {
        private readonly HttpClient _client;
        private readonly ILogger<GameServer> _logger;
        private CancellationTokenSource _cancellationTokenSource;
        private ClientWebSocket _webSocket;

        public GameServer(HttpClient client, IConfiguration configuration, ILogger<GameServer> logger)
        {
            _client = client;
            _client.BaseAddress = new Uri(configuration.GetValue<string>("FunctionsBaseUrl"));
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task LoginAsync(string playerName, Action<JObject> messageHandler)
        {
            var response = await _client.GetAsync($"/api/login/{playerName}");
            var result = await response.Content.ReadAsStringAsync();
            var connectionInfo = JObject.Parse(result);
            var url = new Uri(connectionInfo["url"].Value<string>());

            _webSocket = new ClientWebSocket();
            _webSocket.Options.AddSubProtocol("json.webpubsub.azure.v1");
            await _webSocket.ConnectAsync(url, _cancellationTokenSource.Token);
            _ = ReceiveLoopAsync(messageHandler);
        }

        private async Task ReceiveLoopAsync(Action<JObject> messageHandler)
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var received = await _webSocket.ReceiveAsync(buffer, _cancellationTokenSource.Token);
                if (received.MessageType == WebSocketMessageType.Text)
                {
                    var receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);
                    _logger.LogInformation(receivedAsText);

                    messageHandler(JObject.Parse(receivedAsText));
                }
                else if (received.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Connection closed");
                }
            }
        }

        public Task JoinAsync(string gameSessionId)
        {
            _logger.LogInformation($"Joining game session '{gameSessionId}'...");

            return SendCustomEventAsync("join", new JoinGameSessionCommand
            {
                GameSessionId = gameSessionId
            });
        }

        public Task StartAsync(string gameSessionId)
        {
            _logger.LogInformation($"Starting game session '{gameSessionId}'...");

            return SendCustomEventAsync("start", new StartGameSessionCommand
            {
                GameSessionId = gameSessionId
            });
        }

        public Task SendCustomEventAsync<T>(string eventName, T eventData)
        {
            var message = JsonConvert.SerializeObject(new
            {
                type = "event",
                @event = eventName,
                dataType = "json",
                data = eventData
            });

            Console.WriteLine(message);

            var dataToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            return _webSocket.SendAsync(dataToSend, WebSocketMessageType.Text, true, _cancellationTokenSource.Token);


//            return _websocketClient.SendInstant(message);
        }

        // public Task StartSinglePlayerGameAsync(string gameName, string playerName)
        // {
        //     var command = new StartSinglePlayerGameCommand
        //     {
        //         PlayerName = playerName
        //     };

        //     return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/start", command);
        // }

        // public Task PurchaseAnimalAsync(string gameName, string animalName, string ownerName)
        // {
        //     var command = new PurchaseAnimalCommand
        //     {
        //         AnimalName = animalName,
        //         OwnerName = ownerName
        //     };

        //     return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/purchase-animal", command);
        // }

        // public Task MoveAnimalAsync(string gameName, string animalName, string enclosureName)
        // {
        //     var command = new MoveAnimalCommand
        //     {
        //         AnimalName = animalName,
        //         EnclosureName = enclosureName
        //     };

        //     return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/move-animal", command);
        // }

        // public Task FeedAnimalAsync(string gameName, string playerName, string animalName)
        // {
        //     var command = new FeedAnimalCommand
        //     {
        //         PlayerName = playerName,
        //         AnimalName = animalName
        //     };

        //     return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/feed-animal", command);
        // }

        // public Task WaterAnimalAsync(string gameName, string playerName, string animalName)
        // {
        //     var command = new WaterAnimalCommand
        //     {
        //         PlayerName = playerName,
        //         AnimalName = animalName
        //     };

        //     return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/water-animal", command);
        // }

        // public Task CleanAnimalAsync(string gameName, string playerName, string animalName)
        // {
        //     var command = new CleanAnimalCommand
        //     {
        //         PlayerName = playerName,
        //         AnimalName = animalName
        //     };

        //     return _client.SendJsonAsync(HttpMethod.Post, $"/api/game/{gameName}/clean-animal", command);
        // }

        // private void OnMessageReceived(ResponseMessage message)
        // {
        //     _logger.LogTrace(message.ToString());

        //     MessageReceived?.Invoke(message);
        // }
    }
}