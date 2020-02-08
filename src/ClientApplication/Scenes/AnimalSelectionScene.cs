using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using ClientApplication;
using Microsoft.JSInterop;
using System.Linq;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class AnimalSelectionScene : Scene
    {
        public const string Name = "AnimalSelection";

        private readonly ApiClient _apiClient;

        public AnimalSelectionScene(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop =>
            {
                var ownedAnimals = StateManager.State.Animals
                    .Where(animal => animal.OwnerName == StateManager.PlayerName);

                int index = 0;
                foreach (var animal in ownedAnimals)
                {
                    var animalSpriteName = SpriteName.Create("btnAnimal", animal.Name);

                    interop.AddSprite(animalSpriteName, animal.Name, 200 + (index * 150), 250, options => options
                        .Scale(0.3)
                        .OnPointerUp(nameof(btnAnimal_PointerUp)));
                }

                interop.AddSprite("btnBack", "logo", 700, 100, options => options
                    .Scale(0.3)
                    .OnPointerUp(nameof(btnBack_PointerUp)));
            });
        }

        [JSInvokable]
        public async Task btnAnimal_PointerUp(SpritePointerEventArgs args)
        {
            var animalName = SpriteName.ExtractId(args.SpriteName);

            await _apiClient.MoveAnimal(
                StateManager.State.GameName,
                animalName,
                StateManager.State.SelectedEnclosureName);

            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }

        [JSInvokable]
        public void btnBack_PointerUp(SpritePointerEventArgs args)
        {
            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }
    }
}