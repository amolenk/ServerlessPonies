using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class SpinnerScene : Scene
    {
        public const string Name = "Spinner";

        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop => interop.AddSprite("spinner", "logo", 400, 300));
        }

        // [JSInvokable("destroy")]
        // public override void Destroy()
        // {
        // }
    }
}