
namespace Amolenk.ServerlessPonies.FunctionApplication.Model
{
    public class PlayerState
    {
        private const int DefaultCredits = 1000;

        public string Name { get; set; }

        public int Credits { get; set; }

        public static PlayerState Default(string playerName)
            => new PlayerState
            {
                Name = playerName,
                Credits = DefaultCredits
            };
    }
}