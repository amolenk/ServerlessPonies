using Amolenk.ServerlessPonies.Messages;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
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
            var newOccupant = stateManager.State.FindAnimal(@event.AnimalName);
            if (newOccupant != null)
            {
                var currentOccupant = stateManager.State.FindAnimalInEnclosure(@event.EnclosureName);
                if (currentOccupant != null)
                {
                    if (currentOccupant == newOccupant)
                    {
                        // No change needed.
                        return;
                    }
                    else
                    {
                        // Remove the current occupant of the enclosure.
                        currentOccupant.EnclosureName = null;
                    }
                }

                newOccupant.EnclosureName = @event.EnclosureName;
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