using Amolenk.ServerlessPonies.ClientApplication.Model;
using Microsoft.JSInterop;
using System.Drawing;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class CreditsScene : Scene
    {
        public const string Name = "CreditsScene";

        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            var player = StateManager.State.FindPlayer(StateManager.PlayerName);

            Phaser(interop => interop
                .AddText("txtCredits", 50, 50, player.Credits.ToString(), 32, Color.Red));
        }

        protected override void StateChanged(GameState state)
        {
            var player = StateManager.State.FindPlayer(StateManager.PlayerName);
            player.CreditsChanged += (sender, args) =>
            {
                Phaser(interop => interop.Text("txtCredits").Value(args.Credits.ToString()));
            };
        }
    }
}