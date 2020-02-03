using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    public interface IPlayer
    {
        void StartGame(PlayerInfo playerInfo);

        Task NotifyPlayer(object eventPayload);
    }
}