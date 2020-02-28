using Amolenk.ServerlessPonies.ClientApplication.Model;
using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using Microsoft.JSInterop;
using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class AnimalCareScene : Scene
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

                .AddSprite("sprBackground", "backgrounds/animal-care", 640, 512)

                .AddSprite("sprBack", "misc/board", 180, 60)
                .AddSprite("btnBack", "misc/back", 165, 95, options => options
                    .OnPointerUp(nameof(btnBack_PointerUp)))

                .AddSprite("sprAnimal", $"animals/{State.SelectedAnimal().Name}/side", 750, 750, options => options
                    .OnPointerMove(nameof(Animal_PointerMove))
                    .OnPointerUp(nameof(Animal_PointerUp)))

                .AddSprite("sprMoodBackground", "misc/mood-bg", 640, 940)

                .AddSprite("sprHappinessLevelBackground", "misc/mood-level-bg", 490, 955)
                .AddSprite("sprHungrinessLevelBackground", "misc/mood-level-bg", 640, 955)
                .AddSprite("sprThirstinessLevelBackground", "misc/mood-level-bg", 790, 955)

                .AddSprite("sprHungrinessLevel", "misc/mood-level", 490, 955)
                .AddSprite("sprThirstinessLevel", "misc/mood-level", 640, 955)
                .AddSprite("sprHappinessLevel", "misc/mood-level", 790, 955)

                .AddSprite("btnFeed", "actions/feed", 490, 890, options => options
                    .OnPointerDown(nameof(FeedButton_PointerDown))) 

                .AddSprite("btnWater", "actions/water", 640, 890, options => options
                    .OnPointerDown(nameof(WaterButton_PointerDown)))
                
                .AddSprite("btnClean", "actions/clean", 790, 890, options => options
                    .OnPointerDown(nameof(CleanButton_PointerDown)))

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
                    _apiClient.CleanAnimal(
                        StateManager.State.GameName,
                        StateManager.PlayerName,
                        StateManager.State.SelectedAnimalName);

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
            }
            else if (_activitySprite == "sprWater")
            {
                _apiClient.WaterAnimal(
                    StateManager.State.GameName,
                    StateManager.PlayerName,
                    StateManager.State.SelectedAnimalName);
            }
        }

        [JSInvokable]
        public void CleanButton_PointerDown(SpritePointerEventArgs e)
        {
            Phaser(interop => interop
                .AddSprite("sprClean", "actions/clean-item", e.X, e.Y));

            _activitySprite = "sprClean";
            _cleaningPoints = 0;
        }

        [JSInvokable]
        public void FeedButton_PointerDown(SpritePointerEventArgs e)
        {
            Phaser(interop => interop
                .AddSprite("sprFeed", "actions/feed-item", e.X, e.Y));

            _activitySprite = "sprFeed";
        }

        [JSInvokable]
        public void WaterButton_PointerDown(SpritePointerEventArgs e)
        {
            Phaser(interop => interop
                .AddSprite("sprWater", "actions/water-item", e.X, e.Y));

            _activitySprite = "sprWater";
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

            var player = StateManager.State.FindPlayer(StateManager.PlayerName);
            player.CreditsChanged += (sender, args) =>
            {
                if (args.Delta > 0)
                {
                    Phaser(interop => interop.AddFireworks());
                }
            };
        }

        private void UpdateMoodLevelSprites(Animal animal)
        {
            Phaser(interop => 
            {
                interop.Sprite("sprHungrinessLevel")
                    .Crop(0, 0, CalculateMoodLevelWidth(animal.HungrinessLevel), 10)
                    .Tint(ResolveMoodLevelColor(animal.HungrinessLevel));

                interop.Sprite("sprThirstinessLevel")
                    .Crop(0, 0, CalculateMoodLevelWidth(animal.ThirstinessLevel), 10)
                    .Tint(ResolveMoodLevelColor(animal.ThirstinessLevel));

                interop.Sprite("sprHappinessLevel")
                    .Crop(0, 0, CalculateMoodLevelWidth(animal.HappinessLevel), 10)
                    .Tint(ResolveMoodLevelColor(animal.HappinessLevel));
            });
        }

        private int CalculateMoodLevelWidth(double moodLevel)
            => Math.Max(2, Math.Min(98, (int)(moodLevel * 100)));

        private string ResolveMoodLevelColor(double moodLevel)
            => moodLevel > 0.7 ? "0x28bb25" : moodLevel < 0.3 ? "0xd81919" : "0xd9be2b";
    }
}