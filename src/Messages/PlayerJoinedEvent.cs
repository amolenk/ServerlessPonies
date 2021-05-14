using System;
using System.Collections.Generic;

namespace Amolenk.ServerlessPonies.Messages
{
    public class PlayerJoinedEvent
    {
        public string GameSessionId { get; set; }

        public List<PlayerState> Players { get; set; }
    }
}
