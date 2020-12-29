using OOP.Classes.InterfaceFood;

namespace OOP.Classes.Creatures
{
    public abstract class Omnivorous<TOmni> : Mob<IOmnivorousFood>, IHumanFood, IHunterFood
    where TOmni : IOmnivorousFood
    {
        protected Omnivorous(Ground ground, int x, int y) : base(ground, x, y)
        {
        }
        protected override void SearchCouple()
        {
            Search<Omnivorous<TOmni>>(SearchTargetType.Couple);
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
                Search<TOmni>(SearchTargetType.Food);
            }
        }

        protected override void OutOfMadness()
        {
            if (!Ground.Field[X, Y].IsAffectedByMadness)
            {
                Search<TOmni>(SearchTargetType.Food);
            }
        }
    }
}