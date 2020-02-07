
namespace ClientApplication
{
    public class StateManager : IStateManager
    {
        public StateManager(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; }

        public GameState State { get; set; }
    }
}