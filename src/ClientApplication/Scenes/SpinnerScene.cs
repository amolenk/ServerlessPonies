using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.Scenes
{
    public class SpinnerScene : Scene
    {
        public const string Name = "Spinner";

        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop => interop
                .AddSprite("spinner", "logo", 400, 300));
        }
    }
}