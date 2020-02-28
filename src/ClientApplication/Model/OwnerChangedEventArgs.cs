using System;

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