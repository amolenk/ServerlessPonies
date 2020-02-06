using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using ClientApplication;
using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public abstract class Scene
    {
        private IPhaserInterop _phaser;

        protected Scene()
        {
        }

        // protected SceneManager SceneManager { get; private set; }

        // protected ApiClient ApiClient { get; private set; }

        protected GameState State { get; private set; }

        public void Initialize(IPhaserInterop phaser, GameState state)
        {
            _phaser = phaser;

            this.State = state;
        }

        protected void Phaser(Action<IPhaserSceneInterop> interop)
        {
            interop(_phaser.Scene(this));
        }

        public abstract string GetName();

        public virtual void InitializeState(GameState state)
        {
        }

        public abstract void Create();
    }
}