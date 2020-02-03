using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalPlacedEvent
    {
        //public string PlayerId { get; set; }

        public string AnimalId { get; set; }

        public string EnclosureId { get; set; }
    }
}
