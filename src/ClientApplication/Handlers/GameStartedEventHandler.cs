using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.Messages;
using System.Linq;

namespace Amolenk.ServerlessPonies.ClientApplication.Handlers
{
    public class GameStartedEventHandler : IEventHandler<GameStartedEvent>
    {
        public void Handle(GameStartedEvent @event, IStateManager stateManager)
        {
            stateManager.State = new GameState
            {
                IsStarted = true,
                GameName = @event.GameName,
                Players = @event.Players
                    .Select(playerState => new Player
                    {
                        Name = playerState.Name,
                        Credits = playerState.Credits
                    })
                    .ToList(),
                Animals = @event.Animals
                    .Select(animalState => new Animal
                    {
                        Name = animalState.Name,
                        Price = animalState.Price,
                        OwnerName = animalState.OwnerName,
                        EnclosureName = animalState.EnclosureName,
                        HungrinessLevel = 1,
                        ThirstinessLevel = 1,
                        HappinessLevel = 1
                    })
                    .ToList()
            };
        }
    }
}