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

        public string SelectedAnimalName { get; set; }

        public List<Animal> Animals { get; set; }

        public List<Enclosure> Enclosures { get; set; }

        public Animal SelectedAnimal()
            => Animals.Find(animal => animal.Name == SelectedAnimalName);

        public EventHandler<Animal> AnimalStateChanged;

        public void UpdateAnimalState(string name, Action<Animal> update)
        {
            var animal = Animals.Find(animal => animal.Name == name);
            if (animal != null)
            {
                update(animal);
            }

            AnimalStateChanged?.Invoke(this, animal);
        }
    }
}