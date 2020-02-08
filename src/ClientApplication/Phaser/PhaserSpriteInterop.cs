using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public class PhaserSpriteInterop : IPhaserSpriteInterop
    {
        private readonly IJSInProcessRuntime _jsRuntime;
        private readonly string _sceneName;
        private readonly string _spriteName;

        public PhaserSpriteInterop(IJSInProcessRuntime jsRuntime, string sceneName, string spriteName)
        {
            _jsRuntime = jsRuntime;
            _sceneName = sceneName;
            _spriteName = spriteName;
        }

        public bool Exists()
        {
            return _jsRuntime.Invoke<bool>("spriteExists", _sceneName, _spriteName);
        }

        public IPhaserSpriteInterop Crop(double x, double y, double width, double height)
        {
            _jsRuntime.InvokeVoid("setSpriteCrop", _sceneName, _spriteName, x, y, width, height);
            return this;
        }

        public IPhaserSpriteInterop Move(double x, double y)
        {
            _jsRuntime.InvokeVoid("setSpriteLocation", _sceneName, _spriteName, x, y);
            return this;
        }

        public IPhaserSpriteInterop Scale(double scale)
        {
            _jsRuntime.InvokeVoid("setSpriteScale", _sceneName, _spriteName, scale);
            return this;
        }

        public IPhaserSpriteInterop OnPointerUp(string handlerName)
        {
            _jsRuntime.InvokeVoid("addSpriteEventHandler", _sceneName, _spriteName, "pointerup", handlerName);
            return this;
        }

        public IPhaserSpriteInterop OnPointerDown(string handlerName)
        {
            _jsRuntime.InvokeVoid("addSpriteEventHandler", _sceneName, _spriteName, "pointerdown", handlerName);
            return this;
        }

        public IPhaserSpriteInterop OnPointerMove(string handlerName)
        {
            _jsRuntime.InvokeVoid("addSpriteEventHandler", _sceneName, _spriteName, "pointermove", handlerName);
            return this;
        }
    }
}