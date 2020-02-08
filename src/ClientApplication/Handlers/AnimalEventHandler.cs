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
        IEventHandler2<AnimalPurchasedEvent>
    {
        public void Handle(AnimalMovedEvent @event, IStateManager stateManager)
        {
            var animal = stateManager.State.Animals.Find(animal => animal.Name == @event.AnimalName);
            if (animal != null)
            {
                animal.EnclosureName = @event.EnclosureName;
            }
        }

        public void Handle(AnimalPurchasedEvent @event, IStateManager stateManager)
        {
            var animal = stateManager.State.Animals.Find(animal => animal.Name == @event.AnimalName);
            if (animal != null)
            {
                animal.OwnerName = @event.OwnerName;
            }
        }

        // public void Handle(AnimalMoodChangedEvent @event)
        // {
        //     var animal = State.Animals.Find(animal => animal.Name == @event.AnimalName);
        //     if (animal != null)
        //     {
        //         animal.HappinessLevel = @event.HappinessLevel;
        //         animal.HungrinessLevel = @event.HungrinessLevel;
        //         animal.ThirstinessLevel = @event.ThirstinessLevel;
        //     }
        // }
    }
}