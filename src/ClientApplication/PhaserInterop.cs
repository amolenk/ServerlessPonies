using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    public interface IPhaserInterop
    {
        void RegisterScene(Scene scene);

        void Start(string containerElement, string title);

        IPhaserSceneInterop InScene(Scene scene);
    }

    public interface IPhaserSceneInterop
    {
        IPhaserSceneInterop AddSprite(string name, string imageName, int x, int y, double scale = 1);

        IPhaserSceneInterop SetSpriteScale(string name, double scale);

        IPhaserSceneInterop SetSpriteCrop(string name, int x, int y, int width, int height);

        IPhaserSceneInterop AddRectangle(int x, int y, int width, int height, string color = "black");

        // TODO Create Sync/Async versions
        IPhaserSceneInterop OnPointerUp(string spriteName, string handlerName);

        void StartScene(string name);

        void StopScene(string name);

        void SwitchToScene(string name);
    }

    public class PhaserSceneInterop : IPhaserSceneInterop
    {
        private readonly IJSInProcessRuntime _jsRuntime;
        private readonly Scene _scene;

        public PhaserSceneInterop(IJSInProcessRuntime jsRuntime, Scene scene)
        {
            _jsRuntime = jsRuntime;
            _scene = scene;
        }

        public IPhaserSceneInterop AddSprite(string name, string imageName, int x, int y, double scale = 1)
        {
            _jsRuntime.InvokeVoid("addSprite", _scene.GetName(), name, imageName, x, y, scale);
            return this;
        }

        public IPhaserSceneInterop SetSpriteScale(string name, double scale)
        {
            _jsRuntime.InvokeVoid("setSpriteScale", _scene.GetName(), name, scale);
            return this;
        }

        public IPhaserSceneInterop SetSpriteCrop(string name, int x, int y, int width, int height)
        {
            _jsRuntime.InvokeVoid("setSpriteCrop", _scene.GetName(), name, x, y, width, height);
            return this;
        }

        public IPhaserSceneInterop AddRectangle(int x, int y, int width, int height, string color)
        {
            _jsRuntime.InvokeVoid("addRectangle", _scene.GetName(), x, y, width, height, color);
            return this;
        }

        public IPhaserSceneInterop OnPointerUp(string spriteName, string handlerName)
        {
            _jsRuntime.InvokeVoid("addHandler", _scene.GetName(), spriteName, handlerName);
            return this;
        }

        public void StartScene(string name)
        {
            _jsRuntime.InvokeVoid("startScene", name);
        }

        public void StopScene(string name)
        {
            _jsRuntime.InvokeVoid("stopScene", name);
        }

        public void SwitchToScene(string name)
        {
            _jsRuntime.InvokeVoid("switchScene", _scene.GetName(), name);
        }
    }

    // public class PhaserRectangleInterop : IPhaserRectangleInterop
    // {
    //     private readonly IJSInProcessRuntime _jsRuntime;
    //     private readonly Scene _scene;
    //     private readonly int _width;
    //     private readonly int _height;

    //     public PhaserRectangleInterop(IJSInProcessRuntime jsRuntime, Scene scene, int width, int height)
    //     {
    //         _jsRuntime = jsRuntime;
    //         _scene = scene;
    //         _width = width;
    //         _height = height;
    //     }

    //     public IPhaserSceneInterop PlaceAt(int x, int y)
    //     {
    //         _jsRuntime.InvokeVoid("addRectangle", _scene.GetName(), _width, _height, x, y);

    //         return new PhaserSceneInterop(_jsRuntime, _scene);
    //     }
    // }

    /// <summary>
    /// Generic client class that interfaces .NET Standard/Blazor with SignalR Javascript client
    /// </summary>
    public class PhaserClient : IPhaserInterop
    {
        private readonly IJSInProcessRuntime _jSRuntime;

        public PhaserClient(IJSRuntime jsRuntime)
        {
            _jSRuntime = (IJSInProcessRuntime)jsRuntime;
        }

        public void RegisterScene(Scene scene)
        {
            _jSRuntime.InvokeVoid("registerScene", scene.GetName(), DotNetObjectReference.Create(scene));
        }

        // public void SwitchScene(Scene from, Scene to)
        // {
        //     _jSRuntime.InvokeVoid("switchScene", from.GetName(), to.GetName());
        // }

        public void Start(string containerElement, string title)
        {
            _jSRuntime.InvokeVoid("startPhaser", containerElement, title);
        }

        public IPhaserSceneInterop InScene(Scene scene)
            => new PhaserSceneInterop(_jSRuntime, scene);

        // public IPhaserClient AddSprite(Scene scene, string sprite, string imageName, int x, int y, double scale, bool interactive)
        // {
        //     _jSRuntime.InvokeVoid("addSprite", scene.GetName(), sprite, imageName, x, y, scale, interactive);
        //     return this;
        // }

        // public IPhaserClient AddHandler(Scene scene, string spriteName, string handlerName)
        // {
        //     _jSRuntime.InvokeVoid("addHandler", scene.GetName(), spriteName, handlerName);
        //     return this;
        // }
    }
}