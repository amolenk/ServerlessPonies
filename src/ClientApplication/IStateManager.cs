
using System;

namespace ClientApplication
{
    public interface IStateManager
    {
        string PlayerName { get; }

        GameState State { get; set; }

        event EventHandler GameStateChanged;
    }
}