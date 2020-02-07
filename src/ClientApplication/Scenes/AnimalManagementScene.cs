using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class AnimalManagementScene : Scene,
        IEventHandler<AnimalPurchasedEvent>,
        IEventHandler<AnimalMovedEvent>
    {
        public const string Name = "AnimalManagement";

        private readonly ApiClient _apiClient;
        
        private Dictionary<string, int> _animalPrices;

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

            await _apiClient.MoveAnimal(State.GameName, animalName, State.SelectedEnclosureName);
        }

        [JSInvokable] // TODO Introduce separate sync/async handlers for pointer
        public async Task BuyButton_PointerUp(SpritePointerEventArgs e)
        {
            var animalName = SpriteName.ExtractId(e.SpriteName);

            Console.WriteLine(animalName);

            await _apiClient.PurchaseAnimalAsync(State.GameName, animalName, PlayerName);
        }

        public void Handle(AnimalMovedEvent @event)
        {
            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }

        public void Handle(AnimalPurchasedEvent @event)
        {
            Phaser(interop =>
            {
                var buyButton = SpriteName.Create("buyButton", @event.AnimalName);
                interop.RemoveSprite(buyButton);
            });
        }
    }
}