using System.Drawing;
using System.Linq;
using OOP.Classes.InterfaceFood;
using OOP.Classes.InterfaceFood.IAllPredatorFood;

namespace OOP.Classes.Creatures
{
    public abstract class Grass : Creature, IHumanFood, IHerbivorousFood, IOmnivorousFood
    {
        private const int GrassEveryTick = 1;
        private const int Radius = 5;

        protected Grass(Ground ground, int x, int y)
        {
            Ground = ground;
            X = x;
            Y = y;
            Dead = false;
        }

        protected abstract void CreateGrass(int x, int y);

        public void Grow()
        {
            var newGrass = new Point();
            if (Ground.Rnd.Next(250) != 0) return;
            for (var i = 0; i < GrassEveryTick;)
            {
                newGrass.X = Ground.Rnd.Next(-Radius, Radius);
                newGrass.Y = Ground.Rnd.Next(-Radius, Radius);
                if (Ground.CheckTheEnd(X, Y, newGrass.X, newGrass.Y)
                    && !Ground.Field[X + newGrass.X, Y + newGrass.Y].Objects.Any(x => x is Grass))
                {
                    int newX = X + newGrass.X, newY = Y + newGrass.Y;
                    CreateGrass(newX, newY);
                    i++;
                }
            }
        }
    }
}