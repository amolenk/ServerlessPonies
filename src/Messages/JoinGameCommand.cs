using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amolenk.ServerlessPonies.Messages
{
    public class JoinGameCommand
    {
        public string PlayerName { get; set; }
    }
}