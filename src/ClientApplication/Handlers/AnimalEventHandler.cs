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
    public class AnimalEventHandler : IEventHandler2<AnimalMovedEvent>,
        IEventHandler2<AnimalPurchasedEvent>,
        IEventHandler2<AnimalPurchaseFailedEvent>,
        IEventHandler2<AnimalMoodChangedEvent>
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
                Console.WriteLine("Recd happy: " + @event.HappinessLevel);

                animal.HappinessLevel = @event.HappinessLevel;
                animal.HungrinessLevel = @event.HungrinessLevel;
                animal.ThirstinessLevel = @event.ThirstinessLevel;
            }
        }
    }
}