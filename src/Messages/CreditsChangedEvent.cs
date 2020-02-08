using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class CreditsChangedEvent
    {
        public string PlayerName { get; set; }

        public int Credits { get; set; }
    }
}
