using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amolenk.ServerlessPonies.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Animal : IAnimal
    {
        private const double GameTimeRatio = 0.3;

        private readonly Random _random;

        private readonly IAsyncCollector<SignalRMessage> _signalRMessages;

        public Animal(IAsyncCollector<SignalRMessage> signalRMessages)
        {
            _random = new Random();
            _signalRMessages = signalRMessages;
        }

        [JsonProperty]
        public string OwnerId { get; set; }

        [JsonProperty]
        public MoodLevel HappinessLevel { get; set; } 

        [JsonProperty]
        public MoodLevel HungrinessLevel { get; set; } 

        [JsonProperty]
        public MoodLevel ThirstinessLevel { get; set; }

        [JsonProperty]
        public int? DelayMoodChangeByGameMinutes { get; set; }

        public Task SetOwner(string ownerId)
        {
            OwnerId = ownerId;

            HungrinessLevel = new MoodLevel { Value = 1 };
            ThirstinessLevel = new MoodLevel { Value = 1 };
            HappinessLevel = new MoodLevel { Value = 1 };

            //ScheduleNextMoodChange(15);

            return Task.CompletedTask;
        }
        
        public Task BrushAsync()
        {
            return IncreaseMoodLevelAsync(HappinessLevel, 0.25);
        }

        public Task FeedAsync()
        {
            return IncreaseMoodLevelAsync(HungrinessLevel, 0.25);
        }

        public Task HydrateAsync()
        {
            return IncreaseMoodLevelAsync(ThirstinessLevel, 0.25);
        }

        public async Task ChangeMoodRandomlyAsync()
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

                    await PublishAnimalMoodChangedEventAsync();

                    ScheduleNextMoodChange(10);
                }
            }
        }

        private async Task IncreaseMoodLevelAsync(MoodLevel moodLevel, double amount)
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

                await PublishAnimalMoodChangedEventAsync();
            }
        }

        private void ScheduleNextMoodChange(int gameMinutes)
        {
            var triggerTime = CalculateActualTime(gameMinutes);

            Entity.Current.SignalEntity(
                Entity.Current.EntityId,
                triggerTime,
                nameof(ChangeMoodRandomlyAsync));
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

        private Task PublishAnimalMoodChangedEventAsync()
        {
            var @event = new AnimalMoodChangedEvent
            {
                AnimalId = Entity.Current.EntityKey,
                HappinessLevel = this.HappinessLevel.Value,
                HungrinessLevel = this.HungrinessLevel.Value,
                ThirstinessLevel = this.ThirstinessLevel.Value
            };

            return _signalRMessages.AddAsync(new SignalRMessage
                {
                    Target = "newMessage",
                    Arguments = new[] { MessageEnvelope.Wrap(@event) }
                });
        }

        private bool IsCompletelySatisfied()
            => this.HappinessLevel.Value == 1
                && this.HungrinessLevel.Value == 1
                && this.ThirstinessLevel.Value == 1;

        [FunctionName(nameof(Animal))]
        public static Task Run(
            [EntityTrigger] IDurableEntityContext context,
            [SignalR(HubName = "ponies")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            // if (!context.HasState)
            // {
            //     context.SetState(new Animal(signalRMessages)
            //     {
            //     });
            // }

            return context.DispatchAsync<Animal>(signalRMessages);
        }
    }
}