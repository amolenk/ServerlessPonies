using System.Threading.Tasks;
using Amolenk.ServerlessPonies.FunctionApplication.Model;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    public interface IGame
    {
        void Join(string playerName);

        Task Start();

        Task StartSinglePlayer(string playerName);

        Task PurchaseAnimalAsync(AnimalPurchase purchase);

        Task MoveAnimalAsync(AnimalMovement movement);
    }
}