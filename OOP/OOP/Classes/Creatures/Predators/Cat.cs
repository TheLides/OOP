using OOP.Classes.InterfaceFood.IAllOmnivorousFood;
using OOP.Classes.InterfaceFood.IAllPredatorFood;

namespace OOP.Classes.Creatures.Predators
{
    public class Cat : Predator<ICatFood>, IDogFood
    {
        public Cat(Ground ground, int x, int y) : base(ground, x, y)
        {
        }

        protected override void GiveBirth()
        {
            var child = new Cat(Ground, X, Y);
            Ground.Field[X, Y].Objects.Add(child);
            Ground.Predators.Add(child);
        }
        
    }
}