
using System;
using Amolenk.ServerlessPonies.ClientApplication.Model;

namespace Amolenk.ServerlessPonies.ClientApplication
{
    public class StateManager : IStateManager
    {
        private GameState _state;

        public string PlayerName { get; set; }

        public event EventHandler GameStateChanged;

        public GameState State
        {
            get { return _state; }
            set
            {
                _state = value;
                GameStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

    }
}