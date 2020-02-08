using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    public class Animal
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public string OwnerName { get; set; }

        public string EnclosureName { get; set; }

        public double HappinessLevel { get; set; }

        public double HungrinessLevel { get; set; }

        public double ThirstinessLevel { get; set; }
    }
}