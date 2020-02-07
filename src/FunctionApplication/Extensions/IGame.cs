// using System.Threading.Tasks;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.SignalRService;

// namespace Amolenk.ServerlessPonies.FunctionApplication.Extensions
// {
//     public static class SignalrRExtensions
//     {
//         public static Task AddEventAsync<TMessage, TEvent>(this IAsyncCollector<SignalRMessage> signalRMessages, TEvent @event)
//         {
//            return signalRMessages.AddAsync(new SignalRMessage
//                 {
//                     Target = "handleEvent",
//                     Arguments = new object[] { typeof(TEvent).Name, @event }
//                 });
//         }
//     }
// }