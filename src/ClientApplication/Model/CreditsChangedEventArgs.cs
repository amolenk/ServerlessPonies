using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Model
{
    public class CreditsChangedEventArgs : EventArgs
    {
        public CreditsChangedEventArgs(int credits, int delta)
        {
            Credits = credits;
            Delta = delta;
        }

        public int Credits { get; }

        public int Delta { get; }
    }
}