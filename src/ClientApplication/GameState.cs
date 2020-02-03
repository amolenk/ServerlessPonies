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
        public string SelectedEnclosureId { get; set; }

        public List<Animal> Animals { get; set; }

        public List<Enclosure> Enclosures { get; set; }

        public static GameState NewGame()
        {
            return new GameState
            {
                Animals = new List<Animal>
                {
                    new Animal
                    {
                        Id = "1"
                    },
                    new Animal
                    {
                        Id = "2"
                    }
                },
                Enclosures = new List<Enclosure>
                {
                    new Enclosure
                    {
                        Id = "1",
                        AnimalId = "2"
                    }
                }
            };
        }
    }
}