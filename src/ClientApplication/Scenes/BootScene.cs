using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class BootScene : Scene,
        IEventHandler<GameStartedEvent>
    {
        public const string Name = "Boot";

        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop => interop.AddSprite("spinner", "logo", 400, 300));
        }

        public void Handle(GameStartedEvent @event)
        {
            var state = new GameState
            {
                GameName = @event.GameName,
                Animals = @event.Animals
                    .Select(animalState => new Animal
                    {
                        Name = animalState.Name,
                        Price = animalState.Price,
                        OwnerName = animalState.OwnerName,
                        EnclosureName = animalState.EnclosureName
                    })
                    .ToList()
            };

            SetState(state);

            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }
    }
}