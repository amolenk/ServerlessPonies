using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.Messages
{
    public class PurchaseAnimalCommand
    {
        public string AnimalName { get; set; }

        public string OwnerName { get; set; }
    }
}