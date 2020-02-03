using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalMoodChangedEvent
    {
        //public string PlayerId { get; set; }

        public string AnimalId { get; set; }

        public double HungrinessLevel { get; set; }

        public double ThirstinessLevel { get; set; }

        public double HappinessLevel { get; set; }
    }
}
