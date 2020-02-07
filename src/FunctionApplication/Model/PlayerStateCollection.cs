using System.Collections.ObjectModel;

namespace Amolenk.ServerlessPonies.FunctionApplication.Model
{
    public class PlayerStateCollection : KeyedCollection<string, PlayerState>
    {
        protected override string GetKeyForItem(PlayerState state)
        {
            return state.Name;
        }
    }
}