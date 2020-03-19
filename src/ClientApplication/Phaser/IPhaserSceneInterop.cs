using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public interface IPhaserSceneInterop
    {
        bool IsVisible();

        IPhaserSceneInterop AddSprite(string name, string imageName, double x, double y, Action<IPhaserSpriteInterop> options = null);

        IPhaserSceneInterop AddText(string name, double x, double y, string text, int fontSize, string color, string style = null);

        IPhaserSceneInterop AddText(string name, double x, double y, string text, Action<IPhaserTextInterop> options = null);

        IPhaserSceneInterop AddFireworks();

        IPhaserSceneInterop RemoveSprite(string name);

        IPhaserSpriteInterop Sprite(string name);

        IPhaserTextInterop Text(string name);

        IPhaserSceneInterop OnPointerMove(string handlerName);

        IPhaserSceneInterop OnPointerDown(string handlerName);

        IPhaserSceneInterop OnPointerUp(string handlerName);

        IPhaserSceneInterop StartScene(string name);

        IPhaserSceneInterop StopScene(string name);

        void SwitchToScene(string name);

        IPhaserSceneInterop ShakeCamera();
    }
}