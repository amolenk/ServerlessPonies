using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using ClientApplication;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class AnimalManagementScene : Scene
    {
        public const string Name = "AnimalManagement";

        private readonly ApiClient _apiClient;

        public AnimalManagementScene(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            int index = 0;
            foreach (var animal in State.Animals)
            {
                var spriteName = SpriteName.Create("animal", animal.Name);

                Phaser(interop =>
                {
                    if (animal.OwnerName == PlayerName)
                    {
                        interop.AddSprite(spriteName, "wally", 200 + (index++ * 150), 300, options => options
                            .Scale(0.3)
                            .OnPointerUp(nameof(Animal_PointerUp)));
                    }
                    else
                    {
                        interop.AddSprite(spriteName, "wally", 200 + (index * 150), 300, options => options
                            .Scale(0.3)
                            .OnPointerUp(nameof(Animal_PointerUp)));

                        var buttonName = SpriteName.Create("buyButton", animal.Name);

                        interop.AddSprite(buttonName, "logo", 200 + (index++ * 150), 400, options => options
                            .Scale(0.15)
                            .OnPointerUp(nameof(BuyButton_PointerUp)));
                    }
                });

            }
        }

        [JSInvokable]
        public async Task Animal_PointerUp(SpritePointerEventArgs e)
        {
            var animalName = SpriteName.ExtractId(e.SpriteName);
            var animal = StateManager.State.FindAnimal(animalName);

            if (animal.OwnerName == StateManager.PlayerName)
            {
                await _apiClient.MoveAnimal(State.GameName, animalName, State.SelectedEnclosureName);

                Phaser(interop => interop.SwitchToScene(RanchScene.Name));
            }
            else
            {
                Phaser(interop => interop.ShakeCamera());
            }
        }

        [JSInvokable] // TODO Introduce separate sync/async handlers for pointer
        public async Task BuyButton_PointerUp(SpritePointerEventArgs e)
        {
            var animalName = SpriteName.ExtractId(e.SpriteName);

            await _apiClient.PurchaseAnimalAsync(State.GameName, animalName, PlayerName);
        }

        protected override void StateChanged(GameState state)
        {
            foreach (var animal in state.Animals)
            {
                animal.OwnerChanged += AnimalOwnerChanged;
            }
        }

        private void AnimalOwnerChanged(object sender, OwnerChangedEventArgs e)
        {
            var animal = (Animal)sender;

            Phaser(interop =>
            {
                var buyButton = SpriteName.Create("buyButton", animal.Name);
                interop.RemoveSprite(buyButton);
            });
        }

        private void AnimalStateChanged(object sender, Animal animal)
        {
            Console.WriteLine("[AnimalManagementScene] Animal state changed!: " + animal.Name);
        }
    }
}