using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalHungrinessLevelChangedEvent
    {
        //public string PlayerId { get; set; }

        public string AnimalId { get; set; }

        public double HungrinessLevel { get; set; }
    }
}
