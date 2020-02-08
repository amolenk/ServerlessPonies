using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.FunctionApplication.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AnimalBehavior : IAnimalBehavior
    {
        private const double GameTimeRatio = 0.3;

        private readonly Random _random;

        public AnimalBehavior()
        {
            _random = new Random();
        }

        [JsonProperty]
        public string GameName { get; set; } 

        [JsonProperty]
        public bool IsStarted { get; set; } 

        [JsonProperty]
        public string AnimalName { get; set; }

        [JsonProperty]
        public string OwnerName { get; set; } 

        [JsonProperty]
        public MoodLevel HappinessLevel { get; set; } 

        [JsonProperty]
        public MoodLevel HungrinessLevel { get; set; } 

        [JsonProperty]
        public MoodLevel ThirstinessLevel { get; set; }

        [JsonProperty]
        public int? DelayMoodChangeByGameMinutes { get; set; }

        public void Start()
        {
            IsStarted = true;
            HappinessLevel = new MoodLevel { Value = 1 };
            HungrinessLevel = new MoodLevel { Value = 1 };
            ThirstinessLevel = new MoodLevel { Value = 1 };

            ScheduleNextMoodChange(15);
        }
        
        public void Clean()
        {
            if (IsStarted)
            {
                IncreaseMoodLevel(HappinessLevel, 0.25);
            }
        }

        public void Feed()
        {
            if (IsStarted)
            {
                IncreaseMoodLevel(HungrinessLevel, 0.25);
            }
        }

        public void Water()
        {
            if (IsStarted)
            {
                IncreaseMoodLevel(ThirstinessLevel, 0.25);
            }
        }

        public void ChangeMoodRandomly()
        {
            if (this.DelayMoodChangeByGameMinutes.HasValue)
            {
                ScheduleNextMoodChange(this.DelayMoodChangeByGameMinutes.Value);
                this.DelayMoodChangeByGameMinutes = null;
            }
            else
            {
                var moodLevel = ChooseMoodLevelToUpdate();
                if (moodLevel != null)
                {
                    moodLevel.Decrease(_random.Next(10, 50) / (double)100);

                    UpdateGameState();

                    ScheduleNextMoodChange(10);
                }
            }
        }

        private void IncreaseMoodLevel(MoodLevel moodLevel, double amount)
        {
            if (!IsCompletelySatisfied())
            {
                moodLevel.Increase(amount);

                // If the animal is now completely satisfied, delay the next mood
                // change by 15 game minutes to give the player some rest.
                if (IsCompletelySatisfied())
                {
                    this.DelayMoodChangeByGameMinutes = 15;
                }

                UpdateGameState();
            }
        }

        private void ScheduleNextMoodChange(int gameMinutes)
        {
            var triggerTime = CalculateActualTime(gameMinutes);

            Entity.Current.SignalEntity(
                Entity.Current.EntityId,
                triggerTime,
                nameof(ChangeMoodRandomly));
        }

        private MoodLevel ChooseMoodLevelToUpdate()
        {
            var nonZeroMoodLevels = GetNonZeroMoodLevels().ToList();
            if (nonZeroMoodLevels.Count == 0)
            {
                return null;
            }
            else if (nonZeroMoodLevels.Count == 1)
            {
                return nonZeroMoodLevels[0];
            }
            else
            {
                return nonZeroMoodLevels[_random.Next(nonZeroMoodLevels.Count)];
            }
        }

        private IEnumerable<MoodLevel> GetNonZeroMoodLevels()
        {
            if (HungrinessLevel.Value > 0) yield return HungrinessLevel;
            if (ThirstinessLevel.Value > 0) yield return ThirstinessLevel;
            if (HappinessLevel.Value > 0) yield return HappinessLevel;
        }

        private DateTime CalculateActualTime(int afterGameMinutes)
        {
            var actualSeconds = afterGameMinutes * GameTimeRatio;

            return DateTime.UtcNow.AddSeconds(actualSeconds);
        }

        private void UpdateGameState()
        {
            var moodChange = new AnimalMoodChange
            {
                AnimalName = AnimalName,
                HappinessLevel = HappinessLevel.Value,
                HungrinessLevel = HungrinessLevel.Value,
                ThirstinessLevel = ThirstinessLevel.Value
            };

            var entityId = new EntityId(nameof(Game), GameName);
            Entity.Current.SignalEntity<IGame>(entityId,
                proxy => proxy.UpdateAnimalMoodAsync(moodChange));
        }

        private bool IsCompletelySatisfied()
            => this.HappinessLevel.Value == 1
                && this.HungrinessLevel.Value == 1
                && this.ThirstinessLevel.Value == 1;

        [FunctionName(nameof(AnimalBehavior))]
        public static Task Run(
            [EntityTrigger] IDurableEntityContext ctx)
        {
            if (!ctx.HasState)
            {
                var entityKeyParts = ctx.EntityKey.Split(':');

                ctx.SetState(new AnimalBehavior
                {
                    GameName = entityKeyParts[0],
                    AnimalName = entityKeyParts[1],
                    OwnerName = entityKeyParts[2]
                });
            }
            
            return ctx.DispatchAsync<AnimalBehavior>();
        }
    }
}