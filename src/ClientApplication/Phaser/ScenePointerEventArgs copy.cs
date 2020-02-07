using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
{
    public class ScenePointerEventArgs
    {
        public int X { get; set; }

        public int Y { get; set; }

        public double Distance { get; set; }
    }
}