
namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    public interface IAnimalBehavior
    {
        void Start();

        void Clean();

        void Feed();

        void Water();
    }
}