using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using Microsoft.JSInterop;
using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class AnimalCareScene : Scene,
        IEventHandler<AnimalMoodChangedEvent>
    {
        public const string Name = "AnimalCare";

        private readonly ApiClient _apiClient;

        private string _activitySprite;
        private double _cleaningPoints;

        public AnimalCareScene(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public override string GetName() => Name;

        [JSInvokable("create")]
        public override void Create()
        {
            Phaser(interop => interop
                .AddSprite("sprAnimal", "wally", 400, 300, options => options
                    .OnPointerMove(nameof(Animal_PointerMove))
                    .OnPointerUp(nameof(Animal_PointerUp)))
                
                .AddSprite("sprHappiness", "moodlevel", 150, 50)
                .AddSprite("sprHungriness", "moodlevel", 150, 70)
                .AddSprite("sprThirstiness", "moodlevel", 150, 90)

                .AddSprite("btnClean", "brush", 50, 150, options => options
                    .Scale(0.1)
                    .OnPointerDown(nameof(CleanButton_PointerDown)))
                
                .AddSprite("btnFeed", "logo", 50, 200, options => options
                    .Scale(0.1)
                    .OnPointerDown(nameof(FeedButton_PointerDown)))
                
                .AddSprite("btnWater", "logo", 50, 250, options => options
                    .Scale(0.1)
                    .OnPointerDown(nameof(WaterButton_PointerDown)))

                .AddSprite("btnBack", "logo", 700, 100, options => options
                    .Scale(0.3)
                    .OnPointerUp(nameof(btnBack_PointerUp)))

                .OnPointerMove(nameof(Scene_PointerMove))
                .OnPointerUp(nameof(Scene_PointerUp)));

            UpdateMoodLevelSprites(State.SelectedAnimal());
        }

        [JSInvokable]
        public void btnBack_PointerUp(SpritePointerEventArgs e)
        {
            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }

        [JSInvokable]
        public void Animal_PointerMove(SpritePointerEventArgs e)
        {
            if (_activitySprite == "sprClean")
            {
                _cleaningPoints += e.Distance;
                if (_cleaningPoints >= 10000)
                {
                    Console.WriteLine("Cleaning points!");
                    _cleaningPoints = 0;
                }
            }
        }

        [JSInvokable]
        public void Animal_PointerUp(SpritePointerEventArgs e)
        {
            if (_activitySprite == "sprFeed")
            {
                _apiClient.FeedAnimal(
                    StateManager.State.GameName,
                    StateManager.PlayerName,
                    StateManager.State.SelectedAnimalName);

                Console.WriteLine("Feeding points!");
            }
            else if (_activitySprite == "sprWater")
            {
                Console.WriteLine("Watering points!");
            }
        }

        [JSInvokable]
        public void CleanButton_PointerDown(SpritePointerEventArgs e)
        {
            Phaser(interop => interop
                .AddSprite("sprClean", "brush", e.X, e.Y, options => options
                    .Scale(0.15)));

            _activitySprite = "sprClean";
            _cleaningPoints = 0;
        }

        [JSInvokable]
        public void FeedButton_PointerDown(SpritePointerEventArgs e)
        {
            Phaser(interop => interop
                .AddSprite("sprFeed", "logo", e.X, e.Y, options => options
                    .Scale(0.15)));

            _activitySprite = "sprFeed";
        }

        [JSInvokable]
        public void Scene_PointerMove(ScenePointerEventArgs e)
        {
            if (_activitySprite != null)
            {
                Phaser(interop => interop.Sprite(_activitySprite).Move(e.X, e.Y));
            }
        }

        [JSInvokable]
        public void Scene_PointerUp(ScenePointerEventArgs e)
        {
            Phaser(interop => interop.RemoveSprite(_activitySprite));
            _activitySprite = null;
        }

        [JSInvokable]
        public void WaterButton_PointerDown(SpritePointerEventArgs e)
        {
            Phaser(interop => interop
                .AddSprite("sprWater", "logo", e.X, e.Y, options => options
                    .Scale(0.15)));

            _activitySprite = "sprWater";
        }

        public void Handle(AnimalMoodChangedEvent @event)
        {
            if (@event.AnimalName == State.SelectedAnimalName)
            {
                var animal = State.SelectedAnimal();
                animal.HappinessLevel = @event.HappinessLevel;
                animal.HungrinessLevel = @event.HungrinessLevel;
                animal.ThirstinessLevel = @event.ThirstinessLevel;

                UpdateMoodLevelSprites(animal);
            }
        }

        // TODO Rename to WireStateHandlers?
        protected override void StateChanged(GameState state)
        {
            foreach (var animal in state.Animals)
            {
                animal.MoodChanged += (sender, args) =>
                {
                    if (sender == StateManager.State.SelectedAnimal())
                    {
                        UpdateMoodLevelSprites((Animal)sender);
                    }
                };
            }
        }

        private void UpdateMoodLevelSprites(Animal animal)
        {
            // TODO Use colors for mood levels
            // (https://photonstorm.github.io/phaser3-docs/Phaser.GameObjects.Sprite.html#setTint)
            Phaser(interop => 
            {
                var animal = State.SelectedAnimal();

                Console.WriteLine(animal.Name);
                Console.WriteLine(animal.ThirstinessLevel);

                interop.Sprite("sprHappiness").Crop(0, 0, (int)(animal.HappinessLevel * 100), 10);
                interop.Sprite("sprHungriness").Crop(0, 0, (int)(animal.HungrinessLevel * 100), 10);
                interop.Sprite("sprThirstiness").Crop(0, 0, (int)(animal.ThirstinessLevel * 100), 10);
            });
        }
    }
}