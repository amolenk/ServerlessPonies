using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalThirstinessLevelChangedEvent
    {
        //public string PlayerId { get; set; }

        public string AnimalId { get; set; }

        public double ThirstinessLevel { get; set; }
    }
}
