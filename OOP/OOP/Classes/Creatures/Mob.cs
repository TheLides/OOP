using System.ComponentModel;
using System.Linq;
using OOP.Classes.InterfaceFood;

namespace OOP.Classes.Creatures
{
    public abstract class Mob<TFood> : Creature
        where TFood : IFood
    {
        protected Creature AimFood { get; set; }
        
        protected Mob<TFood> Couple { get; set; }
        protected const int WhenTheyDie = 1000;
        protected const int WhenTheyHungry = 500;
        protected const int WhenTheyWannaChild = 300;
        protected const int Radius = 10;

        protected Mob(Ground ground, int x, int y)
        {
            Ground = ground;
            X = x;
            Y = y;
            GenderThis = (Gender) Ground.Rnd.Next(2);
            Couple = null;
            WantChild = 0;
            Starve = Ground.Rnd.Next(WhenTheyWannaChild);
            Dead = false;
        }

        protected void AimlessMove()
        {
            var value = Ground.Rnd.Next(4);
            int x = 0, y = 0;
            switch (value)
            {
                case 0:
                    x++;
                    break;

                case 1:
                    x--;
                    break;

                case 2:
                    y++;
                    break;

                case 3:
                    y--;
                    break;
            }

            if (!Ground.CheckTheEnd(X, Y, x, y)) return;
            X += x;
            Y += y;
            Ground.Field[X, Y].Objects.Add(this);
            Ground.Field[X - x, Y - y].Objects.Remove(this);
            Starve++;
            WantChild++;
        }

        protected virtual void Search<TSimulationObject>(SearchTargetType type)
        {
            if (!Ground.CheckTheEnd(X, Y, Radius, Radius) || !Ground.CheckTheEnd(X, Y, -Radius, -Radius)) return;
            for (var x = X - Radius; x < X + Radius; x++)
            {
                for (var y = Y - Radius; y < Y + Radius; y++)
                {
                    if (!Ground.Field[x, y].Objects.Any()) continue;
                    foreach (var i in Ground.Field[x, y].Objects)
                    {
                        switch (type)
                        {
                            case SearchTargetType.Couple:
                                if (i is TSimulationObject)
                                {
                                    var aim = i as Mob<TFood>;
                                    if (aim != null && GenderThis != aim.GenderThis)
                                    {
                                        Couple = i as Mob<TFood>;
                                        return;
                                    }
                                }

                                break;
                            case SearchTargetType.Food:
                                if (i is TSimulationObject && i != this)
                                {
                                    AimFood = i as Creature;
                                    return;
                                }

                                break;
                        }
                    }
                }
            }
        }

        protected int GoToX(SimulationObject Aim)
        {
            int newX = X;
            if (Aim.X > X)
            {
                newX = X + 1;
            }

            if (Aim.X < X)
            {
                newX = X - 1;
            }

            return newX;
        }

        protected int GoToY(SimulationObject Aim)
        {
            int newY = Y;
            if (Aim.Y > Y)
            {
                newY = Y + 1;
            }

            if (Aim.Y < Y)
            {
                newY = Y - 1;
            }

            return newY;
        }

        protected void GoToCouple()
        {
            int newX = GoToX(Couple), newY = GoToY(Couple);

            if (X == Couple.X && Y == Couple.Y)
            {
                GiveBirth();
                WantChild = 0;
                Starve++;
                Couple.Starve++;
                Couple.WantChild = 0;
                Couple.Couple = null;
                Couple = null;
            }
            else
            {
                Ground.Field[newX, newY].Objects.Add(this);
                Ground.Field[X, Y].Objects.Remove(this);
                X = newX;
                Y = newY;
                Starve++;
                WantChild++;
            }
        }

        protected abstract void GiveBirth();
        protected abstract void CreatureFindFood();
        protected abstract void OutOfMadness();

        protected virtual void GoToFood()
        {
            int newX = GoToX(AimFood), newY = GoToY(AimFood);

            if (X == AimFood.X && Y == AimFood.Y)
            {
                AimFood.Dead = true;
                Ground.Field[X, Y].Objects.Remove(AimFood);
                Starve = 0;
                AimFood = null;
            }
            else
            {
                Ground.Field[newX, newY].Objects.Add(this);
                Ground.Field[X, Y].Objects.Remove(this);
                X = newX;
                Y = newY;
                Starve++;
                WantChild++;
            }
        }

        protected virtual void SearchFood()
        {
            if (AimFood == null)
            {
                CreatureFindFood();
            }

            if (AimFood != null)
            {
                GoToFood();
                OutOfMadness();
            }
            else
            {
                AimlessMove();
            }
        }

        protected abstract void SearchCouple();

        public virtual void Move()
        {
            if (Starve < WhenTheyHungry && WantChild < WhenTheyWannaChild)
            {
                AimlessMove();
                return;
            }

            if (Starve < WhenTheyHungry && WantChild > WhenTheyWannaChild)
            {
                SearchCouple();
                return;
            }

            if (Starve < WhenTheyDie)
            {
                SearchFood();
            }

            else
            {
                Dead = true;
            }
        }
    }
}