using OOP.Classes.InterfaceFood.IAllOmnivorousFood;

namespace OOP.Classes.Creatures.Omnivore
{
    public class Hedgehog : Omnivorous<IHedgehogFood>
    {
        public Hedgehog(Ground ground, int x, int y) : base(ground, x, y)
        {
        }

        protected override void GiveBirth()
        {
            var child = new Hedgehog(Ground, X, Y);
            Ground.Field[X, Y].Objects.Add(child);
            Ground.Omnivorouses.Add(child);
        }
    }
}