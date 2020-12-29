using System;
using System.Collections.Generic;
using OOP.Classes.Creatures;

namespace OOP.Classes
{
    public class Square
    {
        public List<SimulationObject> Objects { get; set; }

        public bool IsAffectedByMadness { get; set; }
        public Square()
        {
            Objects = new List<SimulationObject>();
            
        }

    }
    
    
}
