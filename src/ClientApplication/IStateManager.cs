
namespace ClientApplication
{
    public interface IStateManager
    {
        string PlayerName { get; }

        GameState State { get; set; }
    }
}