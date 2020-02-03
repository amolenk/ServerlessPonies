using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class MessageEnvelope
    {
        public string Type { get; set; }

        public object Payload { get; set; }

        public static MessageEnvelope Wrap<T>(T message)
        {
            return new MessageEnvelope
            {
                Type = typeof(T).Name,
                Payload = message
            };
        }
    }
}
