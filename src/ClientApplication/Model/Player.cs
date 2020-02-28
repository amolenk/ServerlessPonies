using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Model
{
    public class Player
    {
        private int _credits;

        public string Name { get; set; }

        public int Credits
        {
            get { return _credits; }
            set
            {
                if (value != _credits)
                {
                    var delta = value - _credits;
                    _credits = value;
                    CreditsChanged?.Invoke(this, new CreditsChangedEventArgs(_credits, delta));
                }
            }
        }

        public event EventHandler<CreditsChangedEventArgs> CreditsChanged;
    }
}