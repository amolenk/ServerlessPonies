using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Model
{
    public class EnclosureChangedEventArgs : EventArgs
    {
        public EnclosureChangedEventArgs(string enclosureName)
        {
            EnclosureName = enclosureName;
        }

        public string EnclosureName { get; }
    }
}