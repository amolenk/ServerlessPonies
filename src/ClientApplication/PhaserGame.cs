using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.ClientApplication.Scenes;

namespace ClientApplication
{
    public class PhaserGame
    {
        private readonly IPhaserInterop _phaser;
        private readonly Dictionary<string, Scene> _scenes;

        public PhaserGame(IPhaserInterop phaser, IEnumerable<Scene> scenes)
        {
            _phaser = phaser;
            _scenes = scenes.ToDictionary(scene => scene.GetName());
        }

        public void Start(string containerElement, string title)
        {
            _phaser.Start(containerElement, title);
        }

        public void HandleEvent(object @event)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

            foreach (var scene in _scenes.Values)
            {
                var sceneType = scene.GetType();
                if (handlerType.IsAssignableFrom(sceneType))
                {
                    sceneType.InvokeMember("Handle",
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public,
                        null, scene, new [] { @event });
                }
            }
        }
    }
}