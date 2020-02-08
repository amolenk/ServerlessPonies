using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Model
{
    public class AnimalEventArgs : EventArgs
    {
        public AnimalEventArgs(string animalName)
        {
            AnimalName = animalName;
        }

        public string AnimalName { get; }
    }
}