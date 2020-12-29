using System.Drawing;
using System.Linq;
using OOP.Classes.InterfaceFood;

namespace OOP.Classes.Creatures
{
    public class Human : Mob<IHumanFood>
    {
        private House MyHouse { get; set; }
        private House NeighboringHouse { get; set; }
        public new Human Couple { get; set; }
        private int CollectedFood { get; set; }
        private const int RadiusForHouse = 5;


        public Human(Ground ground, int x, int y) : base(ground, x, y)
        {
            CollectedFood = 0;
            MyHouse = null;
            NeighboringHouse = null;
        }

        protected override void GiveBirth()
        {
            var child = new Human(Ground, X, Y);
            Ground.Field[X, Y].Objects.Add(child);
            Ground.Humans.Add(child);
            WantChild = 0;
            Starve++;
            Couple.Starve++;
            Couple.WantChild = 0;
        }

        protected override void Search<TSimulationObject>(SearchTargetType type)
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
                                    var aim = i as Human;
                                    if (aim != null && GenderThis != aim.GenderThis)
                                    {
                                        Couple = aim;
                                        aim.Couple = this;
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
                            case SearchTargetType.House:
                                if (i is TSimulationObject)
                                {
                                    NeighboringHouse = i as House;
                                    return;
                                }

                                break;
                        }
                    }
                }
            }
        }

        private void GoToNeighboringHouse()
        {
            int newX = GoToX(NeighboringHouse), newY = GoToY(NeighboringHouse);
            if (X == NeighboringHouse.X && Y == NeighboringHouse.Y)
            {
                var newHouse = new Point
                {
                    X = Ground.Rnd.Next(-RadiusForHouse, RadiusForHouse), Y = Ground.Rnd.Next(-RadiusForHouse, RadiusForHouse)
                };
                if (Ground.CheckTheEnd(X, Y, newHouse.X, newHouse.Y)
                    && !Ground.Field[X + newHouse.X, Y + newHouse.Y].Objects.Any(x => x is House))
                {
                    MyHouse = new House(Ground, X + newHouse.X, Y + newHouse.Y, this);
                    Couple.MyHouse = MyHouse;
                    Ground.Field[X + newHouse.X, Y + newHouse.Y].Objects.Add(MyHouse);
                    Ground.Houses.Add(MyHouse);
                }
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

        private void FindHome()
        {
            Search<House>(SearchTargetType.House);
            
            if (NeighboringHouse != null)
            {
                if (!NeighboringHouse.Owners.Any())
                {
                    GoToNeighboringHouse();
                }
                else
                {
                    MyHouse = NeighboringHouse;
                    Couple.MyHouse = NeighboringHouse;
                    MyHouse.Owners.Add(this);
                    MyHouse.Owners.Add(Couple);
                }
            }
            else
            {
                if (MyHouse == null)
                {
                    MyHouse = new House(Ground, X, Y, this);
                    Couple.MyHouse = MyHouse;
                    Ground.Field[X, Y].Objects.Add(MyHouse);
                    Ground.Houses.Add(MyHouse);
                }
            }
        }

        private void GoToHouse()
        {
            int newX = GoToX(MyHouse), newY = GoToY(MyHouse);
            if (X != MyHouse.X && Y != MyHouse.Y)
            {
                Ground.Field[newX, newY].Objects.Add(this);
                Ground.Field[X, Y].Objects.Remove(this);
                X = newX;
                Y = newY;
                Starve++;
                WantChild++;
            }
            else
            {
                AimlessMove();
            }
        }

        protected override void GoToFood()
        {
            int newX = GoToX(AimFood), newY = GoToY(AimFood);

            if (X == AimFood.X && Y == AimFood.Y)
            {
                AimFood.Dead = true;
                Ground.Field[X, Y].Objects.Remove(AimFood);
                CollectedFood++;
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

        private void EndOfHunt()
        {
            if (X != MyHouse.X && Y != MyHouse.Y) return;
            MyHouse.BasementFood += CollectedFood;
            CollectedFood = 0;
        }

        protected override void SearchCouple()
        {
            Search<Human>(SearchTargetType.Couple);
            if (Couple != null && GenderThis == Gender.Male)
            {
                FindHome();
            }
            else
            {
                AimlessMove();
            }

            if (MyHouse != null && Couple != null)
            {
                if (X != MyHouse.X && Y != MyHouse.Y)
                {
                    GoToHouse();
                }
                else
                {
                    if (Couple.X == X && Couple.Y == Y && X == MyHouse.X && Y == MyHouse.Y)
                    {
                        GiveBirth();   
                    }
                    else
                    {
                        AimlessMove();
                    }
                }
            }
            else
            {
                AimlessMove();
            }
        }

        protected override void SearchFood()
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
                if (CollectedFood != 0)
                {
                    if (Starve < WhenTheyDie && Starve > WhenTheyHungry)
                    {
                        Starve = 0;
                        CollectedFood--;
                        CreatureFindFood();
                    }

                    if (MyHouse != null)
                    {
                        GoToHouse();
                        EndOfHunt();
                    }
                }

                AimlessMove();
            }
        }

        protected override void CreatureFindFood()
        {
            if (Ground.Field[X, Y].IsAffectedByMadness)
            {
                Search<Creature>(SearchTargetType.Food);
            }
            else
            {
                if (GenderThis == Gender.Female)
                {
                    Search<Grass>(SearchTargetType.Food);
                }
                else
                {
                    Search<IHunterFood>(SearchTargetType.Food);
                }
            }
        }

        protected override void OutOfMadness()
        {
            if (!Ground.Field[X, Y].IsAffectedByMadness)
            {
                Search<IHumanFood>(SearchTargetType.Food);
            }
        }

        public override void Move()
        {
            if (Starve < WhenTheyHungry && WantChild < WhenTheyWannaChild)
            {
                if (MyHouse == null)
                {
                    AimlessMove();
                }
                else if (X != MyHouse.X && Y != MyHouse.Y)
                {
                    SearchFood();
                }
            }

            if (Starve < WhenTheyHungry && WantChild > WhenTheyWannaChild)
            {
                SearchCouple();
                return;
            }

            if (Starve < WhenTheyDie)
            {
                if (MyHouse != null)
                {
                    if (MyHouse.BasementFood != 0)
                    {
                        GoToHouse();
                        if (X == MyHouse.X && Y == MyHouse.Y)
                        {
                            Starve = 0;
                            MyHouse.BasementFood--;
                        }
                    }
                    else
                    {
                        SearchFood();
                    }
                }
                else
                {
                    SearchFood();
                }
            }
            else
            {
                Dead = true;
            }
        }
    }
}