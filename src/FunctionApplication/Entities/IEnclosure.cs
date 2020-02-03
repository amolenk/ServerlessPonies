using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    public interface IEnclosure
    {
        Task<string> GetOwnerId();

        Task PlaceAnimal(string animalId);
    }
}