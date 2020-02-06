using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public interface IPhaserSpriteInterop
    {
        IPhaserSpriteInterop Crop(int x, int y, int width, int height);

        IPhaserSpriteInterop Move(int x, int y);

        IPhaserSpriteInterop Scale(double scale);

        IPhaserSpriteInterop OnPointerUp(string handlerName);

        IPhaserSpriteInterop OnPointerDown(string handlerName);

        IPhaserSpriteInterop OnPointerMove(string handlerName);
    }
}