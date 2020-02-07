using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class RanchScene : Scene,
        IEventHandler<AnimalMovedEvent>,
        IEventHandler<AnimalMoodChangedEvent>
    {
        public const string Name = "Ranch";

        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop =>
            {
                interop
                    .AddSprite("world", "world", 400, 300, options => options.Scale(0.2))
                    .AddSprite("button", "brush", 100, 125, options => options
                        .Scale(0.2)                    
                        .OnPointerUp(nameof(Button_PointerUp)));

                foreach (var animal in State.Animals.Where(animal => animal.EnclosureName != null))
                {
                    AddAnimalSprite(animal);
                }
            });
        }

        [JSInvokable]
        public Task Button_PointerUp(SpritePointerEventArgs e)
        {
            State.SelectedEnclosureName = "1";

            Phaser(interop => interop.SwitchToScene(AnimalManagementScene.Name));

            return Task.CompletedTask;
        }

        [JSInvokable]
        public Task Animal_PointerUp(SpritePointerEventArgs e)
        {
//            State.SelectedEnclosureId = "1";

            Phaser(interop => interop.SwitchToScene(AnimalCareScene.Name));

            return Task.CompletedTask;
        }

        public void Handle(AnimalMovedEvent @event)
        {
            var animal = State.Animals.Find(animal => animal.Name == @event.AnimalName);
            if (animal != null)
            {
                animal.EnclosureName = @event.EnclosureName;

                AddAnimalSprite(animal);
            }
        }

        public void Handle(AnimalMoodChangedEvent @event)
        {
            // TODO Update in state.

            // TODO Update on screen.

            Console.WriteLine($"Animal mood changed: [happiness: {@event.HappinessLevel}] [hungriness: {@event.HungrinessLevel}] [thirstiness: {@event.ThirstinessLevel}]");
        }

        private void AddAnimalSprite(Animal animal)
        {
            Phaser(interop =>
            {
                 var animalSpriteName = SpriteName.Create("animal", animal.Name);

                interop
                    .AddSprite(animalSpriteName, "logo", 320, 80, options => options
                        .Scale(0.2)
                        .OnPointerUp(nameof(Animal_PointerUp)));
            });
        }
    }
}