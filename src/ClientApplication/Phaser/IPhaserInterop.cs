using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public interface IPhaserInterop
    {
        void RegisterScene(Scene scene);

        void Start(string containerElement, string title, PhaserGame game);

        IPhaserSceneInterop Scene(Scene scene);
    }
}