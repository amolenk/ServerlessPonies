using System.Collections.ObjectModel;

namespace Amolenk.ServerlessPonies.FunctionApplication.Model
{
    public class AnimalStateCollection : KeyedCollection<string, AnimalState>
    {
        protected override string GetKeyForItem(AnimalState state)
        {
            return state.Name;
        }

        public static AnimalStateCollection InitialGameState()
        {
            var result = new AnimalStateCollection();
            result.Add(new AnimalState { Name = "amigo", Price = 500 });
            result.Add(new AnimalState { Name = "boy", Price = 500 });
            result.Add(new AnimalState { Name = "spot", Price = 750 });

            return result;
        }
    }
}