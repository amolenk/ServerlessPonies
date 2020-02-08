
namespace Amolenk.ServerlessPonies.FunctionApplication.Model
{
    public class AnimalState
    {
        public AnimalState()
        {
            HappinessLevel = 1;
            HungrinessLevel = 1;
            ThirstinessLevel = 1;
        }

        public string Name { get; set; }

        public int Price { get; set; }

        public string OwnerName { get; set; }

        public string EnclosureName { get; set; }

        public double HappinessLevel { get; set; } 

        public double HungrinessLevel { get; set; } 

        public double ThirstinessLevel { get; set; }
    }
}