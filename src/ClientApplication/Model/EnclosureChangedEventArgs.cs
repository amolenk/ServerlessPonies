using System;

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