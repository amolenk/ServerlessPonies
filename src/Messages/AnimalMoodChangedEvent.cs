using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalMoodChangedEvent
    {
        public string AnimalName { get; set; }

        public double HappinessLevel { get; set; }

        public double HungrinessLevel { get; set; }

        public double ThirstinessLevel { get; set; }

    }
}
