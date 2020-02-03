using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalHappinessLevelChangedEvent
    {
        //public string PlayerId { get; set; }

        public string AnimalId { get; set; }

        public double HappinessLevel { get; set; }
    }
}
