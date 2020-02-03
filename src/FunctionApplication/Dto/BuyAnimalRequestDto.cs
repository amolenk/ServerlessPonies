using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.FunctionApplication.Dto
{
    public class BuyAnimalRequestDto
    {
        public string PlayerId { get; set; }

        public string AnimalId { get; set; }

        public string EnclosureId { get; set; }
    }
}