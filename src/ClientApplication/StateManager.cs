
using System;
using Amolenk.ServerlessPonies.ClientApplication.Model;

namespace ClientApplication
{
    public class StateManager : IStateManager
    {
        private GameState _state;
        
        public StateManager(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; }

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