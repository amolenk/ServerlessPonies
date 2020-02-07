using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.FunctionApplication.Model
{
    public class GameState
    {
        public string GameName { get; set; }
        public PlayerStateCollection Players { get; set; }
    }
}