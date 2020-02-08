using Amolenk.ServerlessPonies.ClientApplication.Model;
using System;

namespace ClientApplication
{
    public class Animal
    {
        private string _ownerName;
        private string _enclosureName;

        public string Name { get; set; }

        public int Price { get; set; }

        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                if (value != _ownerName)
                {
                    _ownerName = value;
                    OwnerChanged?.Invoke(this, new OwnerChangedEventArgs(_ownerName));
                }
            }
        }

        public string EnclosureName
        {
            get { return _enclosureName; }
            set
            {
                if (value != _enclosureName)
                {
                    _enclosureName = value;
                    EnclosureChanged?.Invoke(this, new EnclosureChangedEventArgs(_enclosureName));
                }
            }
        }

        public double HappinessLevel { get; set; }

        public double HungrinessLevel { get; set; }

        public double ThirstinessLevel { get; set; }

        public event EventHandler<EnclosureChangedEventArgs> EnclosureChanged;

        public event EventHandler<OwnerChangedEventArgs> OwnerChanged;
    }
}