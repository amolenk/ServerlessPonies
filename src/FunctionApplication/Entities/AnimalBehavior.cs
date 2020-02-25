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
        private const int PointsNeededForExtraCredits = 5;

        private const int ExtraCredits = 100;

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
        public int CreditPoints { get; set; }

        [JsonProperty]
        public int? DelayMoodChangeByGameMinutes { get; set; }

        public void Start()
        {
            IsStarted = true;
            HappinessLevel = new MoodLevel { Value = 1 };
            HungrinessLevel = new MoodLevel { Value = 1 };
            ThirstinessLevel = new MoodLevel { Value = 1 };
            CreditPoints = 0;

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

                    UpdateAnimalMoodGameState();

                    if (!IsCompletelyUnsatisfied())
                    {
                        ScheduleNextMoodChange(10);
                    }
                }
            }
        }

        private void IncreaseMoodLevel(MoodLevel moodLevel, double amount)
        {
            if (moodLevel.Value < 1)
            {
                // If we're at rock bottom, start scheduling mood changes again.
                if (IsCompletelyUnsatisfied())
                {
                    ScheduleNextMoodChange(30);
                }

                moodLevel.Increase(amount);
                UpdateAnimalMoodGameState();

                // If the animal is now completely satisfied, delay the next mood
                // change by 15 game minutes to give the player some rest.
                if (IsCompletelySatisfied())
                {
                    this.DelayMoodChangeByGameMinutes = 15;
                }

                if (++this.CreditPoints >= PointsNeededForExtraCredits
                    && IsSatisfied())
                {
                    DepositCreditsToGameState(ExtraCredits);
                    this.CreditPoints = 0;
                }
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

        private void UpdateAnimalMoodGameState()
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

        private void DepositCreditsToGameState(int amount)
        {
            var creditsDeposit = new CreditsDeposit
            {
                PlayerName = OwnerName,
                Amount = amount
            };

            var entityId = new EntityId(nameof(Game), GameName);
            Entity.Current.SignalEntity<IGame>(entityId,
                proxy => proxy.DepositCreditsAsync(creditsDeposit));
        }

        private bool IsSatisfied()
            => this.HappinessLevel.Value >= 0.7
                && this.HungrinessLevel.Value >= 0.7
                && this.ThirstinessLevel.Value >= 0.7;

        private bool IsCompletelySatisfied()
            => this.HappinessLevel.Value == 1
                && this.HungrinessLevel.Value == 1
                && this.ThirstinessLevel.Value == 1;

        private bool IsCompletelyUnsatisfied()
            => this.HappinessLevel.Value == 0
                && this.HungrinessLevel.Value == 0
                && this.ThirstinessLevel.Value == 0;

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