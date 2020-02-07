
namespace Amolenk.ServerlessPonies.FunctionApplication.Model
{
    public class AnimalMoodState
    {
        private const double DefaultLevel = 1;

        public double HappinessLevel { get; set; } 

        public double HungrinessLevel { get; set; } 

        public double ThirstinessLevel { get; set; }

        public static AnimalMoodState Default()
            => new AnimalMoodState
            {
                HappinessLevel = DefaultLevel,
                HungrinessLevel = DefaultLevel,
                ThirstinessLevel = DefaultLevel
            };
    }
}