using OOP.Classes.InterfaceFood.IAllHerbivorousFood;
using OOP.Classes.InterfaceFood.IAllOmnivorousFood;

namespace OOP.Classes.Creatures.Plant
{
    public class Carrot : Grass, IRabbitFood, ISheepFood, IHedgehogFood, IDogFood, IBearFood
    {
        public Carrot(Ground ground, int x, int y) : base(ground, x, y)
        {
            Ground = ground;
            X = x;
            Y = y;
        }

        protected override void CreateGrass(int x, int y)
        {
            var cr = new Carrot(Ground, x, y);
            Ground.Field[x, y].Objects.Add(cr);
            Ground.AllGrass.Add(cr);
        }
    }
}