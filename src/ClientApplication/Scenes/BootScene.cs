using Microsoft.JSInterop;
using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class BootScene : Scene
    {
        public const string Name = "Boot";

        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop => interop.AddSprite("spinner", "logo", 400, 300));

            StateManager.GameStateChanged += (sender, e) =>
            {
                Console.WriteLine("BootScene: GameStateChanged");

                // TODO Double check GameName as well?
                if (StateManager.State.IsStarted)
                {
                    Phaser(interop => interop
                        .StartScene(CreditsScene.Name)
                        .SwitchToScene(RanchScene.Name));
                }
            };
        }
    }
}