using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.JSInterop;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public class PhaserInterop : IPhaserInterop
    {
        private readonly IJSInProcessRuntime _jSRuntime;

        public PhaserInterop(IJSRuntime jsRuntime)
        {
            _jSRuntime = (IJSInProcessRuntime)jsRuntime;
        }

        public void RegisterScene(Scene scene)
        {
            _jSRuntime.InvokeVoid("registerScene", scene.GetName(), DotNetObjectReference.Create(scene));
        }

        public void Start(string containerElement, string title, PhaserGame game)
        {
            _jSRuntime.InvokeVoid("startPhaser", containerElement, title, DotNetObjectReference.Create(game));
        }

        public IPhaserSceneInterop Scene(Scene scene)
            => new PhaserSceneInterop(_jSRuntime, scene);
    }
}