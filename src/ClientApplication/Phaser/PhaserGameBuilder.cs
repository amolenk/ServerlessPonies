using System;
using System.Collections.Generic;
using System.Linq;
using Amolenk.ServerlessPonies.ClientApplication.Scenes;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public class PhaserGameBuilder
    {
        private readonly IPhaserInterop _phaser;
        private readonly IServiceProvider _serviceProvider;
        private IGameServer _gameServer;
        private IStateManager _stateManager;
        private List<Type> _sceneTypes; 
        private Dictionary<Type, Action<object, IStateManager>> _handlers;
        private Action _onPreloadCompleted;

        public PhaserGameBuilder(IPhaserInterop phaser, IServiceProvider serviceProvider)
        {
            _phaser = phaser;
            _serviceProvider = serviceProvider;
            _sceneTypes = new List<Type>();
            _handlers = new Dictionary<Type, Action<object, IStateManager>>();
            _onPreloadCompleted = () => {};
        }

        public PhaserGameBuilder WithGameServer(IGameServer gameServer)
        {
            _gameServer = gameServer;
            return this;
        }

        public PhaserGameBuilder WithStateManager(IStateManager stateManager)
        {
            _stateManager = stateManager;
            return this;
        }

        public PhaserGameBuilder WithScene<T>() where T : Scene
        {
            _sceneTypes.Add(typeof(T));
            return this;
        }

        public PhaserGameBuilder WithEventHandler<T>(IEventHandler<T> handler)
        {
            _handlers.Add(typeof(T), (@event, stateManager) => handler.Handle((T)@event, stateManager));
            return this;
        }

        public PhaserGameBuilder WithPreloadCompletedHandler(Action onPreloadCompleted)
        {
            _onPreloadCompleted = onPreloadCompleted;
            return this;
        }

        public PhaserGame Build()
        {
            return new PhaserGame(
                _phaser,
                _stateManager,
                _sceneTypes.Select(sceneType => RegisterSceneInstance(sceneType, _stateManager)),
                _handlers,
                _onPreloadCompleted);
        }

        private Scene RegisterSceneInstance(Type sceneType, IStateManager stateManager)
        {
            var scene = (Scene)_serviceProvider.GetService(sceneType);
            if (scene == null)
            {
                throw new InvalidOperationException($"Scene of type '{sceneType}' could not be instantiated."
                    + " Make sure it's properly configured with the dependency injection framework.");
            }

            scene.Initialize(_phaser, stateManager, _gameServer);

            _phaser.RegisterScene(scene);

            return scene;
        }
    }
}