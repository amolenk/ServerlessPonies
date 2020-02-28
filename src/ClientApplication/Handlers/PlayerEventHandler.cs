using Amolenk.ServerlessPonies.Messages;
using ClientApplication;

namespace Amolenk.ServerlessPonies.ClientApplication.Handlers
{
    public class PlayerEventHandler : IEventHandler<CreditsChangedEvent>
    {
        public void Handle(CreditsChangedEvent @event, IStateManager stateManager)
        {
            var player = stateManager.State.FindPlayer(@event.PlayerName);
            if (player != null)
            {
                player.Credits = @event.Credits;
            }
        }
    }
}