using Amolenk.ServerlessPonies.ClientApplication.Phaser;
using Amolenk.ServerlessPonies.Messages;
using ClientApplication;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Scenes
{
    public class AnimalCareScene : Scene,
        IEventHandler<AnimalMoodChangedEvent>
    {
        public const string Name = "AnimalCare";

        private const string SPRITE_ANIMAL = "animal";
        private const string SPRITE_CLEAN = "clean";
        private const string SPRITE_CLEAN_BUTTON = "btnClean";
        private const string SPRITE_FEED = "feed";
        private const string SPRITE_FEED_BUTTON = "btnFeed";
        private const string SPRITE_WATER = "water";
        private const string SPRITE_WATER_BUTTON = "btnWater";

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
            // TODO Set mood levels according to state.

            Phaser(interop => interop
                .AddSprite(SPRITE_ANIMAL, "wally", 400, 300, options => options
                    .OnPointerMove(nameof(Animal_PointerMove))
                    .OnPointerUp(nameof(Animal_PointerUp)))
                
                .AddSprite("backToRanch", "logo", 700, 100, options => options
                    .Scale(0.3)
                    .OnPointerUp(nameof(BackToRanch_PointerUp)))
                
                .AddSprite("happinessLevel", "moodlevel", 150, 50)
                .AddSprite("hungrinessLevel", "moodlevel", 150, 70)
                .AddSprite("thirstinessLevel", "moodlevel", 150, 90)

                .AddSprite(SPRITE_CLEAN_BUTTON, "brush", 50, 50, options => options
                    .Scale(0.1)
                    .OnPointerDown(nameof(CleanButton_PointerDown)))
                
                .AddSprite(SPRITE_FEED_BUTTON, "logo", 50, 100, options => options
                    .Scale(0.1)
                    .OnPointerDown(nameof(FeedButton_PointerDown)))
                
                .AddSprite(SPRITE_WATER_BUTTON, "logo", 50, 150, options => options
                    .Scale(0.1)
                    .OnPointerDown(nameof(WaterButton_PointerDown)))
                
                .OnPointerMove(nameof(Scene_PointerMove))
                .OnPointerUp(nameof(Scene_PointerUp)));
        }

        [JSInvokable]
        public void BackToRanch_PointerUp(SpritePointerEventArgs e)
        {
            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }

        [JSInvokable]
        public void Animal_PointerMove(SpritePointerEventArgs e)
        {
            if (_activitySprite == SPRITE_CLEAN)
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
            if (_activitySprite == SPRITE_FEED)
            {
                Console.WriteLine("Feeding points!");
            }
            else if (_activitySprite == SPRITE_WATER)
            {
                Console.WriteLine("Watering points!");
            }
        }

        [JSInvokable]
        public void CleanButton_PointerDown(SpritePointerEventArgs e)
        {
            Phaser(interop => interop
                .AddSprite(SPRITE_CLEAN, "brush", e.X, e.Y, options => options
                    .Scale(0.15)));

            _activitySprite = SPRITE_CLEAN;
            _cleaningPoints = 0;
        }

        [JSInvokable]
        public void FeedButton_PointerDown(SpritePointerEventArgs e)
        {
            Phaser(interop => interop
                .AddSprite(SPRITE_FEED, "logo", e.X, e.Y, options => options
                    .Scale(0.15)));

            _activitySprite = SPRITE_FEED;
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
                .AddSprite(SPRITE_WATER, "logo", e.X, e.Y, options => options
                    .Scale(0.15)));

            _activitySprite = SPRITE_WATER;
        }

        public void Handle(AnimalMoodChangedEvent @event)
        {
            // TODO Use colors for mood levels
            // (https://photonstorm.github.io/phaser3-docs/Phaser.GameObjects.Sprite.html#setTint)
            Phaser(interop => 
            {
                interop.Sprite("happinessLevel").Crop(0, 0, (int)(@event.HappinessLevel * 100), 10);
                interop.Sprite("hungrinessLevel").Crop(0, 0, (int)(@event.HungrinessLevel * 100), 10);
                interop.Sprite("thirstinessLevel").Crop(0, 0, (int)(@event.ThirstinessLevel * 100), 10);
            });
        }
    }
}