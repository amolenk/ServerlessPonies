using Amolenk.ServerlessPonies.ClientApplication.Scenes;
using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Handlers
{
    public class GameStartedEventHandler : IEventHandler2<GameStartedEvent>
    {
        public void Handle(GameStartedEvent @event, IStateManager stateManager)
        {
            stateManager.State = new GameState
            {
                IsStarted = true,
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
        }
    }
}