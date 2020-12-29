using OOP.Classes.InterfaceFood.IAllHerbivorousFood;
using OOP.Classes.InterfaceFood.IAllOmnivorousFood;

namespace OOP.Classes.Creatures.Plant
{
    public class Oats : Grass, IDeerFood, ISheepFood, IHedgehogFood
    {
        public Oats(Ground ground, int x, int y) : base(ground, x, y)
        {
            Ground = ground;
            X = x;
            Y = y;
        }

        protected override void CreateGrass(int x, int y)
        {
            var cr = new Oats(Ground, x, y);
            Ground.Field[x, y].Objects.Add(cr);
            Ground.AllGrass.Add(cr);
        }
    }
}