using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    public class GameState
    {
        public string GameName { get; set; }

        public string SelectedEnclosureName { get; set; }

        public List<Animal> Animals { get; set; }

        public List<Enclosure> Enclosures { get; set; }
    }
}