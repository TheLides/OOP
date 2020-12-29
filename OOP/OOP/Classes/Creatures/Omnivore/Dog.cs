using OOP.Classes.InterfaceFood.IAllOmnivorousFood;

namespace OOP.Classes.Creatures.Omnivore
{
    public class Dog : Omnivorous<IDogFood>
    {
        public Dog(Ground ground, int x, int y) : base(ground, x, y)
        {
        }
        protected override void GiveBirth()
        {
            var child = new Dog(Ground, X, Y);
            Ground.Field[X, Y].Objects.Add(child);
            Ground.Omnivorouses.Add(child);
        }
    }
}