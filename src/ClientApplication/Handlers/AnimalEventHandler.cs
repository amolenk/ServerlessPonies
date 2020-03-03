using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Handlers
{
    public class AnimalEventHandler : IEventHandler<AnimalMovedEvent>,
        IEventHandler<AnimalPurchasedEvent>,
        IEventHandler<AnimalPurchaseFailedEvent>,
        IEventHandler<AnimalMoodChangedEvent>
    {
        public void Handle(AnimalMovedEvent @event, IStateManager stateManager)
        {
            var animal = stateManager.State.FindAnimal(@event.AnimalName);
            if (animal != null)
            {
                animal.EnclosureName = @event.EnclosureName;
            }
        }

        public void Handle(AnimalPurchasedEvent @event, IStateManager stateManager)
        {
            var animal = stateManager.State.FindAnimal(@event.AnimalName);
            if (animal != null)
            {
                animal.OwnerName = @event.OwnerName;
            }
        }

        public void Handle(AnimalPurchaseFailedEvent @event, IStateManager stateManager)
        {
            var animal = stateManager.State.FindAnimal(@event.AnimalName);
            if (animal != null)
            {
                animal.NotifyPurchaseFailed();
            }
        }

        public void Handle(AnimalMoodChangedEvent @event, IStateManager stateManager)
        {
            var animal = stateManager.State.FindAnimal(@event.AnimalName);
            if (animal != null)
            {
                animal.HappinessLevel = @event.HappinessLevel;
                animal.HungrinessLevel = @event.HungrinessLevel;
                animal.ThirstinessLevel = @event.ThirstinessLevel;
            }
        }
    }
}