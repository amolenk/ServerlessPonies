using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.ClientApplication.Model
{
    public class OwnerChangedEventArgs : EventArgs
    {
        public OwnerChangedEventArgs(string ownerName)
        {
            OwnerName = ownerName;
        }

        public string OwnerName { get; }
    }
}