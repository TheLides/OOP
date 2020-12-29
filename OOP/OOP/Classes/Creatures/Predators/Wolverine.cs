using OOP.Classes.InterfaceFood.IAllOmnivorousFood;
using OOP.Classes.InterfaceFood.IAllPredatorFood;

namespace OOP.Classes.Creatures.Predators
{
    public class Wolverine : Predator<IWolverineFood>, IBearFood, IDogFood
    {
        public Wolverine(Ground ground, int x, int y) : base(ground, x, y)
        {
        }

        protected override void GiveBirth()
        {
            var child = new Wolverine(Ground, X, Y);
            Ground.Field[X, Y].Objects.Add(child);
            Ground.Predators.Add(child);
        }
    }
}