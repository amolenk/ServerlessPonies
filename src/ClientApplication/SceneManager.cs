// using Microsoft.JSInterop;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;

// namespace ClientApplication
// {
//     public class SceneManager<TState>
//     {
//         private readonly IPhaserClient _phaser;
//         private readonly ApiClient _apiClient;
//         private readonly IServiceProvider _serviceProvider;
//         private readonly Dictionary<string, Scene> _scenes;
//         private TState _state;

//         public SceneManager(IPhaserClient phaser, ApiClient apiClient, IServiceProvider serviceProvider)
//         {
//             _phaser = phaser;
//             _apiClient = apiClient;
//             _serviceProvider = serviceProvider;
//             _scenes = new Dictionary<string, Scene>();
//         }

//         public SceneManager<TState> RegisterScene<T>() where T : Scene
//         {
//             var scene = (Scene)_serviceProvider.GetService(typeof(T));

//             _phaser.RegisterScene(scene);
//             _scenes.Add(scene.GetName(), scene);

//             return this;
//         }

//         public void HandleEvent(object @event)
//         {
//             var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

//             foreach (var scene in _scenes.Values)
//             {
//                 var sceneType = scene.GetType();
//                 if (handlerType.IsAssignableFrom(sceneType))
//                 {
//                     sceneType.InvokeMember("Handle",
//                         BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public,
//                         null, scene, new [] { @event });
//                 }
//             }
//         }

//         public void Start(string containerElement, string title, TState state)
//         {
//             foreach (var scene in _scenes.Keys)
//             {
//                 _scenes[scene]
//             }

//             _state = state;
//             _phaser.Start(containerElement, title);
//         }

//         private 
//     }
// }