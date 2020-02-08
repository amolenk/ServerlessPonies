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
        bool Exists();

        IPhaserSpriteInterop Crop(double x, double y, double width, double height);

        IPhaserSpriteInterop Move(double x, double y);

        IPhaserSpriteInterop Scale(double scale);

        IPhaserSpriteInterop OnPointerUp(string handlerName);

        IPhaserSpriteInterop OnPointerDown(string handlerName);

        IPhaserSpriteInterop OnPointerMove(string handlerName);
    }
}