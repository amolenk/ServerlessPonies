using System;

namespace Amolenk.ServerlessPonies.Messages
{
    public class MoveAnimalCommand
    {
        public string AnimalName { get; set; }

        public string EnclosureName { get; set; }
    }
}