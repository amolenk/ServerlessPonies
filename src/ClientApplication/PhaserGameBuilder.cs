using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientApplication
{
    public class PhaserGameBuilder
    {
        private readonly IPhaserInterop _phaser;
        private readonly IServiceProvider _serviceProvider;
        private GameState _state;
        private List<Type> _sceneTypes; 

        public PhaserGameBuilder(IPhaserInterop phaser, IServiceProvider serviceProvider)
        {
            _phaser = phaser;
            _serviceProvider = serviceProvider;
            _sceneTypes = new List<Type>();
        }

        public PhaserGameBuilder WithState(GameState state)
        {
            _state = state;
            return this;
        }

        public PhaserGameBuilder WithScene<T>() where T : Scene
        {
            _sceneTypes.Add(typeof(T));
            return this;
        }

        public PhaserGame Build()
        {
            return new PhaserGame(_phaser, _sceneTypes.Select(RegisterSceneInstance));
        }

        private Scene RegisterSceneInstance(Type sceneType)
        {
            var scene = (Scene)_serviceProvider.GetService(sceneType);
            scene.Initialize(_phaser, _state);

            _phaser.RegisterScene(scene);

            return scene;
        }
    }
}