namespace OOP.Classes.Creatures
{
    public abstract class Creature : SimulationObject
    {
        public bool Dead { get; set; }
        public Gender GenderThis { get; set; }
        public int Starve { get; set; }
        public int WantChild { get; set; }
    }
}