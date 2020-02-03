using Amolenk.ServerlessPonies.Messages;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.Scenes
{
    public class AnimalCareScene : Scene,
        IEventHandler<AnimalMoodChangedEvent>
    {
        public const string Name = "AnimalCare";

        private readonly ApiClient _apiClient;

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
                .AddSprite("animal", "wally", 400, 300)
                .AddSprite("backToRanch", "logo", 700, 100, 0.3)
                .AddSprite("happinessLevel", "moodlevel", 150, 50)
                .AddSprite("hungrinessLevel", "moodlevel", 150, 70)
                .AddSprite("thirstinessLevel", "moodlevel", 150, 90)
                .OnPointerUp("backToRanch", nameof(BackToRanch_PointerUp)));
        }

        [JSInvokable]
        public void BackToRanch_PointerUp(string spriteName)
        {
            Phaser(interop => interop.SwitchToScene(RanchScene.Name));
        }

        public void Handle(AnimalMoodChangedEvent @event)
        {
            // TODO Use colors for mood levels
            // (https://photonstorm.github.io/phaser3-docs/Phaser.GameObjects.Sprite.html#setTint)
            Phaser(interop => interop
                .SetSpriteCrop("happinessLevel", 0, 0, (int)(@event.HappinessLevel * 100), 10)
                .SetSpriteCrop("hungrinessLevel", 0, 0, (int)(@event.HungrinessLevel * 100), 10)
                .SetSpriteCrop("thirstinessLevel", 0, 0, (int)(@event.ThirstinessLevel * 100), 10));
        }
    }
}