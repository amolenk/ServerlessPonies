using System;
using System.Collections.Generic;

namespace Amolenk.ServerlessPonies.Messages
{
    public class GameStartedEvent
    {
        public string GameName { get; set; }

        public List<AnimalState> Animals { get; set; }
    }
}
