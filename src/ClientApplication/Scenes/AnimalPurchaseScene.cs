using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using ClientApplication;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class AnimalPurchaseScene : Scene
    {
        public const string Name = "AnimalPurchase";

        private readonly ApiClient _apiClient;

        public AnimalPurchaseScene(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop => interop
                .AddSprite("btnBack", "logo", 700, 100, options => options
                    .Scale(0.3)
                    .OnPointerUp(nameof(btnBack_PointerUp))));

            foreach (var animal in State.Animals)
            {
                UpdateAnimalSprites(animal);
            }
        }

        [JSInvokable] // TODO Introduce separate sync/async handlers for pointer
        public void btnBack_PointerUp(SpritePointerEventArgs args)
        {
            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }

        [JSInvokable] // TODO Introduce separate sync/async handlers for pointer
        public async Task btnPurchase_PointerUp(SpritePointerEventArgs args)
        {
            var animalName = SpriteName.ExtractId(args.SpriteName);
            var animal = StateManager.State.FindAnimal(animalName);

            Phaser(interop => interop.RemoveSprite(args.SpriteName));

            EventHandler handler = null;
            handler = (sender, args) =>
            {
                animal.PurchaseFailed -= handler; // Unsubscribe

                Phaser(interop => interop.ShakeCamera());
                UpdateAnimalSprites(animal);
            };
            animal.PurchaseFailed += handler;

            await _apiClient.PurchaseAnimalAsync(State.GameName, animalName, PlayerName);
        }

        private void UpdateAnimalSprites(Animal animal)
        {
            Phaser(interop =>
            {
                var index = StateManager.State.Animals.IndexOf(animal);

                if (!interop.Sprite(animal.Name).Exists())
                {
                    interop.AddSprite(animal.Name, animal.Name, 200 + (index * 150), 300, options => options
                        .Scale(0.3));
                }

                if (animal.OwnerName == null)
                {
                    var buttonName = SpriteName.Create("btnPurchase", animal.Name);
                    if (!interop.Sprite(buttonName).Exists())
                    {
                        interop.AddSprite(buttonName, "logo", 200 + (index++ * 150), 400, options => options
                            .Scale(0.15)
                            .OnPointerUp(nameof(btnPurchase_PointerUp)));
                    }
                }
            });
        }
    }
}