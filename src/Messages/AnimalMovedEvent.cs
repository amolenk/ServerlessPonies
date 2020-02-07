using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalMovedEvent
    {
        public string AnimalName { get; set; }

        public string EnclosureName { get; set; }
    }
}
