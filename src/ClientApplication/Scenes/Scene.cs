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

        public void Initialize(IPhaserInterop phaser, IStateManager stateManager)
        {
            _phaser = phaser;
            _stateManager = stateManager;

            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        protected void Phaser(Action<IPhaserSceneInterop> interop)
        {
            var scene = _phaser.Scene(this);
            if (scene.IsVisible())
            {
                Console.WriteLine($"Scene '{this.GetName()}' is visible.");
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