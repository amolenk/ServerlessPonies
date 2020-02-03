using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    public interface IAnimal
    {
        Task SetOwner(string ownerId);
        
        Task BrushAsync();

        Task FeedAsync();

        Task HydrateAsync();
    }
}