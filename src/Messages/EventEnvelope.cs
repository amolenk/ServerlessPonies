using Newtonsoft.Json.Linq;

namespace Amolenk.ServerlessPonies.Messages
{
    public class EventEnvelope
    {
        public string EventType { get; set; }

        public object EventBody { get; set; }
    }
}
