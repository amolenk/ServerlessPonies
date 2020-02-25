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
            Phaser(interop =>
            {
                interop
                    .AddSprite("sprBackground", "backgrounds/animal-management", 640, 512)
                    .AddSprite("sprBack", "misc/board", 180, 60)
                    .AddSprite("btnBack", "misc/back", 165, 95, options => options
                        .OnPointerUp(nameof(btnBack_PointerUp)));

                for (var i = 0; i < State.Animals.Count; i++)
                {
                    var animal = State.Animals[i];
                    var xPosition = (i + 1) * 250;

                    interop.AddSprite(SpriteName.Create("btnAnimalPhoto", animal.Name), $"animals/{animal.Name}/photo",
                        xPosition + 50, 350, options => options
                            .OnPointerUp(nameof(btnAnimalPhoto_PointerUp)));

                    if (animal.OwnerName != StateManager.PlayerName)
                    {
                        interop.AddSprite(SpriteName.Create("sprLock", animal.Name), "misc/lock",
                            xPosition + 143, 467);
                    }

                    var label = animal.OwnerName != null ? animal.Name : animal.Price.ToString();

                    interop.AddText(SpriteName.Create("txtPhoto", animal.Name),
                        xPosition + 50, 430, label, options => options
                            .WithOrigin(0.5, 0.5));
                }
            });
        }

        [JSInvokable]
        public void btnBack_PointerUp(SpritePointerEventArgs args)
        {
            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }

        [JSInvokable]
        public async Task btnAnimalPhoto_PointerUp(SpritePointerEventArgs args)
        {
            var animalName = SpriteName.ExtractId(args.SpriteName);
            var animal = StateManager.State.FindAnimal(animalName);

            if (animal.OwnerName == null)
            {
                Phaser(interop => interop
                    .Text(SpriteName.Create("txtPhoto", animal.Name))
                        .Value("purchasing..."));

                await _apiClient.PurchaseAnimalAsync(State.GameName, animalName, PlayerName);
            }
            else if (animal.OwnerName == StateManager.PlayerName)
            {
                await _apiClient.MoveAnimal(State.GameName, animalName, State.SelectedEnclosureName);

                Phaser(interop => interop.SwitchToScene(RanchScene.Name));
            }
            else
            {
                Phaser(interop => interop.ShakeCamera());
            }
        }

        protected override void StateChanged(GameState state)
        {
            foreach (var animal in state.Animals)
            {
                animal.OwnerChanged += AnimalOwnerChanged;
                animal.PurchaseFailed += AnimalPurchaseFailed;
            }
        }

        private void AnimalOwnerChanged(object sender, OwnerChangedEventArgs e)
        {
            var animal = (Animal)sender;

            var lockSpriteName = SpriteName.Create("sprLock", animal.Name);

            Phaser(interop =>
            {
                if (interop.Sprite(lockSpriteName).Exists())
                {
                    interop.RemoveSprite(lockSpriteName);
                }

                interop.Text(SpriteName.Create("txtPhoto", animal.Name))
                    .Value(animal.Name);
            });
        }

        private void AnimalPurchaseFailed(object sender, EventArgs e)
        {
            var animal = (Animal)sender;

            Phaser(interop => interop
                .ShakeCamera()
                .Text(SpriteName.Create("txtPhoto", animal.Name))
                    .Value(animal.Price.ToString()));
        }
    }
}