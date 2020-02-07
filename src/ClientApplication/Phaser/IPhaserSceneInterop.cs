using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public interface IPhaserSceneInterop
    {
        bool IsVisible();

        IPhaserSceneInterop AddSprite(string name, string imageName, int x, int y, Action<IPhaserSpriteInterop> options = null);

        IPhaserSceneInterop RemoveSprite(string name);

        IPhaserSpriteInterop Sprite(string name);

        IPhaserSceneInterop AddRectangle(int x, int y, int width, int height, string color = "black");

        // TODO Create Sync/Async versions

        IPhaserSceneInterop OnPointerMove(string handlerName);

        IPhaserSceneInterop OnPointerDown(string handlerName);

        IPhaserSceneInterop OnPointerUp(string handlerName);

        void StartScene(string name);

        void StopScene(string name);

        void SwitchToScene(string name);
    }
}