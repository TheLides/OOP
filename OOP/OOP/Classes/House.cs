using System.Collections.Generic;
using OOP.Classes.Creatures;

namespace OOP.Classes
{
    public class House : SimulationObject
    {
        public int BasementFood { get; set; }
        public List<Human> Owners { get; set; }

        public House(Ground ground, int x, int y)
        {
            Ground = ground;
            X = x;
            Y = y;
            Owners = new List<Human>();
            BasementFood = 0;
        }
        public House(Ground ground, int x, int y, Human builder) : this(ground, x, y)
        {
            Owners.Add(builder);
            Owners.Add(builder.Couple);
        }
    }
}