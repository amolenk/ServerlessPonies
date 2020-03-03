using Amolenk.ServerlessPonies.ClientApplication.Model;
using ClientApplication;
using Microsoft.JSInterop;
using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class BootScene : Scene
    {
        public const string Name = "Boot";

        private readonly ApiClient _apiClient;

        public BootScene(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop => interop.AddSprite("sprLogo", "misc/logo", 640, 512));

            _apiClient.StartSinglePlayerGameAsync(Guid.NewGuid().ToString("N"), StateManager.PlayerName);
        }

        protected override void WireStateHandlers(GameState state)
        {
            // TODO Double check GameName as well?
            if (StateManager.State.IsStarted)
            {
                Phaser(interop => interop
                    .StartScene(CreditsScene.Name)
                    .SwitchToScene(RanchScene.Name));
            }
        }
    }
}



