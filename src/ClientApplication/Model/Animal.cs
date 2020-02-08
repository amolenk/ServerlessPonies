using System;

namespace Amolenk.ServerlessPonies.ClientApplication.Model
{
    public class Animal
    {
        private string _ownerName;
        private string _enclosureName;
        private double _happinessLevel;
        private double _hungrinessLevel;
        private double _thirstinessLevel;

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

        public double HappinessLevel
        {
            get { return _happinessLevel; }
            set
            {
                if (value != _happinessLevel)
                {
                    _happinessLevel = value;
                    MoodChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public double HungrinessLevel
        {
            get { return _hungrinessLevel; }
            set
            {
                if (value != _hungrinessLevel)
                {
                    _hungrinessLevel = value;
                    MoodChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public double ThirstinessLevel
        {
            get { return _thirstinessLevel; }
            set
            {
                if (value != _thirstinessLevel)
                {
                    _thirstinessLevel = value;
                    MoodChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler<OwnerChangedEventArgs> OwnerChanged;

        public event EventHandler<EnclosureChangedEventArgs> EnclosureChanged;

        public event EventHandler MoodChanged;

        public event EventHandler PurchaseFailed;

        public void NotifyPurchaseFailed()
        {
            Console.WriteLine("NotifyPurchaseFailed");

            PurchaseFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}