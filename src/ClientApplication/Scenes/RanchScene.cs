using Amolenk.ServerlessPonies.ClientApplication.Model;
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
    public class RanchScene : Scene
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
                        .OnPointerUp(nameof(Button_PointerUp)))
                    .AddSprite(SpriteName.Create("btnEnclosure", "1"), "logo", 400, 300, options => options
                        .Scale(0.3)
                        .OnPointerUp(nameof(btnEnclosure_PointerUp)));

                foreach (var animal in State.Animals)
                {
                    if (animal.EnclosureName != null)
                    {
                        AddAnimalSprite(animal);
                    }

                }
            });
        }

        [JSInvokable] // TODO Sync/async
        public Task Button_PointerUp(SpritePointerEventArgs e)
        {
            State.SelectedEnclosureName = "1";

            Phaser(interop => interop.SwitchToScene(AnimalPurchaseScene.Name));

            return Task.CompletedTask;
        }

        [JSInvokable] // TODO Sync/async
        public void btnEnclosure_PointerUp(SpritePointerEventArgs e)
        {
            State.SelectedEnclosureName = SpriteName.ExtractId(e.SpriteName);

            Phaser(interop => interop.SwitchToScene(AnimalSelectionScene.Name));
        }

        [JSInvokable]
        public Task Animal_PointerUp(SpritePointerEventArgs e)
        {
            State.SelectedAnimalName = SpriteName.ExtractId(e.SpriteName);

            Phaser(interop => interop.SwitchToScene(AnimalCareScene.Name));

            return Task.CompletedTask;
        }

        protected override void StateChanged(GameState state)
        {
            foreach (var animal in state.Animals)
            {
                animal.EnclosureChanged += AnimalEnclosureChanged;
            }
        }

        private void AnimalEnclosureChanged(object sender, EnclosureChangedEventArgs e)
        {
            var animal = (Animal)sender;

            AddAnimalSprite(animal);
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