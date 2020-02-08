using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Model
{
    public class CreditsChangedEventArgs : EventArgs
    {
        public CreditsChangedEventArgs(int credits)
        {
            Credits = credits;
        }

        public int Credits { get; }
    }
}