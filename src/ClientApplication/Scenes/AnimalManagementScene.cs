using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class AnimalManagementScene : Scene,
        IEventHandler<AnimalPlacedEvent>
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
                var spriteName = SpriteName.Create("animal", animal.Id);

                Phaser(interop => interop
                    .AddSprite(spriteName, "logo", 200 + (index++ * 150), 300, options => options
                    .Scale(0.3)
                    .OnPointerUp(nameof(Animal_PointerUp))));
            }
        }

        [JSInvokable]
        public async Task Animal_PointerUp(string spriteName, int x, int y)
        {
            var animalId = SpriteName.ExtractId(spriteName);

            Console.WriteLine($"Placing animal '{animalId}' in enclosure '{State.SelectedEnclosureId}'!");

            await _apiClient.PlaceAnimal(animalId, State.SelectedEnclosureId);
        }

        public void Handle(AnimalPlacedEvent @event)
        {
            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }
    }
}