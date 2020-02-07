using System.Collections.ObjectModel;

namespace Amolenk.ServerlessPonies.FunctionApplication.Model
{
    public class AnimalStateCollection : KeyedCollection<string, AnimalState>
    {
//        public AnimalStateCollection(IEnumerable)

        protected override string GetKeyForItem(AnimalState state)
        {
            return state.Name;
        }

        public static AnimalStateCollection InitialGameState()
        {
            var result = new AnimalStateCollection();
            result.Add(new AnimalState { Name = "wally", Price = 500 });

            return result;
        }
    }
}