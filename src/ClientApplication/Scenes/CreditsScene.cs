using Amolenk.ServerlessPonies.ClientApplication.Model;
using Microsoft.JSInterop;

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
                .AddSprite("sprCreditsBg", "misc/board", 1100, 60)
                .AddSprite("sprCreditsCoin", "misc/coin", 1025, 95)
                .AddText("txtCredits", 1055, 95, player.Credits.ToString(), 36, "black"/*Color.FromArgb(237, 227, 211)*/, "bold"));
        }

        protected override void WireStateHandlers(GameState state)
        {
            var player = StateManager.State.FindPlayer(StateManager.PlayerName);
            player.CreditsChanged += (sender, args) =>
            {
                Phaser(interop => interop.Text("txtCredits").Value(args.Credits.ToString()));
            };
        }
    }
}