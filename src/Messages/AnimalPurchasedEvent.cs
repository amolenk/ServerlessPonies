using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalPurchasedEvent
    {
        public string AnimalName { get; set; }

        public string OwnerName { get; set; }
    }
}
