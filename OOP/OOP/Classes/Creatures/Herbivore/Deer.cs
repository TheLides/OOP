using OOP.Classes.InterfaceFood.IAllHerbivorousFood;
using OOP.Classes.InterfaceFood.IAllOmnivorousFood;
using OOP.Classes.InterfaceFood.IAllPredatorFood;

namespace OOP.Classes.Creatures.Herbivore
{
    public class Deer : Herbivorous<IDeerFood>, IWolfFood, IWolverineFood, IBearFood
    {
        public Deer(Ground ground, int x, int y) : base(ground, x, y)
        {
        }

        protected override void GiveBirth()
        {
            var child = new Deer(Ground, X, Y);
            Ground.Field[X, Y].Objects.Add(child);
            Ground.Herbivorouses.Add(child);
        }
    }
}