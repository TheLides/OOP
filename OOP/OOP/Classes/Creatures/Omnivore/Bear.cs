using OOP.Classes.InterfaceFood.IAllOmnivorousFood;

namespace OOP.Classes.Creatures.Omnivore
{
    public class Bear : Omnivorous<IBearFood>
    {
        public Bear(Ground ground, int x, int y) : base(ground, x, y)
        {
        }

        protected override void GiveBirth()
        {
            var child = new Bear(Ground, X, Y);
            Ground.Field[X, Y].Objects.Add(child);
            Ground.Omnivorouses.Add(child);
        }
    }
}