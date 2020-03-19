using System;
using System.Collections.Generic;

namespace Amolenk.ServerlessPonies.ClientApplication.Model
{
    public class GameState
    {
        public string GameName { get; set; }

        public bool IsStarted { get; set; }

        public string SelectedEnclosureName { get; set; }

        public string SelectedAnimalName { get; set; }

        public List<Player> Players { get; set; }

        public List<Animal> Animals { get; set; }

        public List<Enclosure> Enclosures { get; set; }
        
        public Animal SelectedAnimal()
            => Animals.Find(animal => animal.Name == SelectedAnimalName);

        public Player FindPlayer(string name)
            => Players.Find(player => player.Name == name);

        public Animal FindAnimal(string name)
            => Animals.Find(animal => animal.Name == name);
    }
}