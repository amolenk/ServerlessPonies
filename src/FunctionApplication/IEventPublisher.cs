using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.FunctionApplication
{
    public interface IEventPublisher
    {
        Task PublishAsync(string playerId, object eventPayload);
    }
}