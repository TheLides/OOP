using OOP.Classes.InterfaceFood;
using OOP.Classes.InterfaceFood.IAllPredatorFood;

namespace OOP.Classes.Creatures
{
    public abstract class Herbivorous<THerb> : Mob<IHerbivorousFood>, IHumanFood, IPredatorFood, IOmnivorousFood, IHunterFood
    where THerb : IHerbivorousFood
    {
        protected Herbivorous(Ground ground, int x, int y) : base(ground, x, y)
        {
        }
        protected override void SearchCouple()
        {
            Search<Herbivorous<THerb>>(SearchTargetType.Couple);
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
                Search<THerb>(SearchTargetType.Food);
            }
        }
        protected override void OutOfMadness()
        {
            if (!Ground.Field[X, Y].IsAffectedByMadness)
            {
                Search<THerb>(SearchTargetType.Food);
            }
        }
    }
}