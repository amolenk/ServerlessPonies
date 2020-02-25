using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using ClientApplication;
using Newtonsoft.Json;
using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public abstract class Scene
    {
        private IPhaserInterop _phaser;
        private IStateManager _stateManager;

        protected Scene()
        {
        }

        protected string PlayerName => _stateManager.PlayerName;

        protected GameState State => _stateManager.State;

        protected IStateManager StateManager => _stateManager;

        public void Initialize(IPhaserInterop phaser, IStateManager stateManager)
        {
            _phaser = phaser;
            _stateManager = stateManager;

            OnInitialized();
        }

        // Actual scenes may not need this anymore
        protected virtual void OnInitialized()
        {
            StateManager.GameStateChanged += (sender, args) =>
            {
                StateChanged(StateManager.State);
            };
        }

        protected virtual void StateChanged(GameState state)
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

        public abstract string GetName();

        public abstract void Create();

        protected void SetState(GameState state)
        {
            _stateManager.State = state;
        }
    }
}