using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public abstract class Scene
    {
        private IPhaserInterop _phaser;

        protected Scene()
        {
        }

        protected string PlayerName => StateManager.PlayerName;

        protected GameState State => StateManager.State;

        protected IStateManager StateManager { get; private set; }

        protected IGameServer GameServer { get; private set; }

        public void Initialize(IPhaserInterop phaser, IStateManager stateManager, IGameServer gameServer)
        {
            _phaser = phaser;

            StateManager = stateManager;
            GameServer = gameServer;

            StateManager.GameStateChanged += (sender, args) =>
            {
                WireStateHandlers(StateManager.State);
            };
        }

        public abstract string GetName();

        public abstract void Create();

        protected virtual void WireStateHandlers(GameState state)
        {
        }

        protected void Phaser(Action<IPhaserSceneInterop> interop)
        {
            var scene = _phaser.Scene(this);
            if (scene.IsVisible())
            {
                interop(scene);
            }
        }

        protected void SetState(GameState state)
        {
            StateManager.State = state;
        }
    }
}