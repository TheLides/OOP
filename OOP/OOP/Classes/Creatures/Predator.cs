using OOP.Classes.InterfaceFood;

namespace OOP.Classes.Creatures
{
    public abstract class Predator<TPred> : Mob<IPredatorFood>, IHumanFood, IHunterFood
    where TPred : IPredatorFood
    {
        protected Predator(Ground ground, int x, int y) : base(ground, x, y)
        {
        }
        protected override void SearchCouple()
        {
            Search<Predator<TPred>>(SearchTargetType.Couple);
            if (Couple == null)
            {
                AimlessMove();
            }
            else
            {
                GoToCouple();
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
                Search<TPred>(SearchTargetType.Food);
            }
        }
        
        protected override void OutOfMadness()
        {
            if (!Ground.Field[X, Y].IsAffectedByMadness)
            {
                Search<TPred>(SearchTargetType.Food);
            }
        }
    }
}