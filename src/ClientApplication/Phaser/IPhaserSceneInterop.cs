using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public interface IPhaserSceneInterop
    {
        bool IsVisible();

        IPhaserSceneInterop AddSprite(string name, string imageName, double x, double y, Action<IPhaserSpriteInterop> options = null);

        IPhaserSceneInterop AddText(string name, double x, double y, string text, int fontSize, Color color);

        IPhaserSceneInterop RemoveSprite(string name);

        IPhaserSpriteInterop Sprite(string name);

        IPhaserTextInterop Text(string name);

        IPhaserSceneInterop AddRectangle(double x, double y, double width, double height, string color = "black");

        // TODO Create Sync/Async versions

        IPhaserSceneInterop OnPointerMove(string handlerName);

        IPhaserSceneInterop OnPointerDown(string handlerName);

        IPhaserSceneInterop OnPointerUp(string handlerName);

        IPhaserSceneInterop StartScene(string name);

        IPhaserSceneInterop StopScene(string name);

        void SwitchToScene(string name);

        void ShakeCamera();
    }
}