using OOP.Classes.InterfaceFood.IAllHerbivorousFood;
using OOP.Classes.InterfaceFood.IAllOmnivorousFood;
using OOP.Classes.InterfaceFood.IAllPredatorFood;

namespace OOP.Classes.Creatures.Predators
{
    public class Wolf : Predator<IWolfFood>, IBearFood
    {
        public Wolf(Ground ground, int x, int y) : base(ground, x, y)
        {
        }

        protected override void GiveBirth()
        {
            var child = new Wolf(Ground, X, Y);
            Ground.Field[X, Y].Objects.Add(child);
            Ground.Predators.Add(child);
        }
    }
}