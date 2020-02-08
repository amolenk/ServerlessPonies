using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class AnimalPurchaseFailedEvent
    {
        public string AnimalName { get; set; }

        public string OwnerName { get; set; }
    }
}
