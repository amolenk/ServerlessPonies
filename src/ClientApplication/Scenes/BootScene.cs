using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.Messages;
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
        }

        protected override void WireStateHandlers(GameState state)
        {
            // FIX Double check GameName as well?
            if (StateManager.State.IsStarted)
            {
                Console.WriteLine("Boot: IsStarted");

                Phaser(interop => interop
                    .StartScene(CreditsScene.Name)
                    .SwitchToScene(RanchScene.Name));
            }
        }
    }
}



