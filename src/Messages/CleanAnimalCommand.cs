using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class CleanAnimalCommand
    {
        public string PlayerName { get; set; }

        public string AnimalName { get; set; }
    }
}
