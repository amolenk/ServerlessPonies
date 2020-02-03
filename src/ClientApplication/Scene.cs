using ClientApplication.Scenes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
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
            interop(_phaser.InScene(this));
        }

        public abstract string GetName();

        public virtual void InitializeState(GameState state)
        {
        }

        public abstract void Create();
    }
}