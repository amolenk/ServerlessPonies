using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace Amolenk.ServerlessPonies.FunctionApplication
{
    public static class SignalRFunctions
    {
        // When the client app opens, it requires valid connection credentials to connect to the
        // Azure SignalR service. This function will return the connection information.
        [FunctionName("GetSignalRInfo")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "signalr/{userId}/negotiate")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "ponies", UserId = "{userId}")] SignalRConnectionInfo connectionInfo)
            => connectionInfo;

        [FunctionName("events")]
        public static Task PublishEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "events")] object payload,
            [SignalR(HubName = "ponies")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "newMessage",
                    Arguments = new[] { payload }
                });
        }
    }
}
