
using System;
using Amolenk.ServerlessPonies.ClientApplication.Model;

namespace ClientApplication
{
    public interface IStateManager
    {
        string PlayerName { get; }

        GameState State { get; set; }

        event EventHandler GameStateChanged;
    }
}