using System;

namespace Amolenk.ServerlessPonies.FunctionApplication.Entities
{
    public class MoodLevel
    {
        public double Value { get; set; }

        public void Increase(double amount)
        {
            this.Value = Math.Min(1, this.Value + amount);
        }

        public void Decrease(double amount)
        {
            this.Value = Math.Max(0, this.Value - amount);
        }
    }
}