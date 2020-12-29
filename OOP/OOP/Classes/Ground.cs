using System;
using System.Collections.Generic;
using OOP.Classes.Creatures;
using OOP.Classes.Creatures.Herbivore;
using OOP.Classes.Creatures.Omnivore;
using OOP.Classes.Creatures.Plant;
using OOP.Classes.Creatures.Predators;
using OOP.Classes.InterfaceFood;
using OOP.Classes.InterfaceFood.IAllPredatorFood;

namespace OOP.Classes
{
    public class Ground
    {
        private int Row { get; set; }
        private int Col { get; set; }
        private int AmountHerbivorous { get; set; }
        private int AmountPredator { get; set; }
        private int AmountHuman { get; set; }
        private int AmountGrass { get; set; }
        private int AmountOmnivorous { get; set; }
        private int AmountHouses { get; set; }
        public Square[,] Field;
        public Random Rnd = new Random();
        public List<Mob<IHerbivorousFood>> Herbivorouses;
        public List<Mob<IPredatorFood>> Predators;
        public List<Mob<IOmnivorousFood>> Omnivorouses;
        public List<Human> Humans;
        public List<Grass> AllGrass;
        public List<House> Houses;
        private const int SizeOfGround = 1000;

        public Ground(int row, int col)
        {
            Row = row;
            Col = col;
            AmountHerbivorous = 1500;
            AmountPredator = 1500;
            AmountOmnivorous = 1500;
            AmountHuman = 750;
            Field = new Square[Row, Col];
            AmountGrass = 1251;
            AmountHouses = 250;

            Predators = new List<Mob<IPredatorFood>>();

            Herbivorouses = new List<Mob<IHerbivorousFood>>();
            
            Omnivorouses = new List<Mob<IOmnivorousFood>>();

            Humans = new List<Human>();

            AllGrass = new List<Grass>();
            
            Houses = new List<House>();

            for (var x = 0; x < Col; x++)
            {
                for (var y = 0; y < Row; y++)
                {
                    Field[x, y] = new Square();
                }
            }

            for (var i = 0; i < AmountHerbivorous; i++)
            {
                int randX = Rnd.Next(1000), randY = Rnd.Next(1000);
                Mob<IHerbivorousFood> cr;
                if (i < AmountHerbivorous / 3)
                {
                    cr = new Deer(this, randX, randY);
                }
                else if (i < (AmountHerbivorous / 3)*2)
                {
                    cr = new Sheep(this, randX, randY);
                }
                else
                {
                    cr = new Rabbit(this, randX, randY);
                }
                Herbivorouses.Add(cr);
                Field[randX, randY].Objects.Add(cr);
            }

            for (var i = 0; i < AmountPredator; i++)
            {
                int randX = Rnd.Next(1000), randY = Rnd.Next(1000);
                Mob<IPredatorFood> cr;
                if (i < AmountHerbivorous / 3)
                {
                    cr = new Cat(this, randX, randY);
                }
                else if (i < (AmountHerbivorous / 3)*2)
                {
                    cr = new Wolverine(this, randX, randY);
                }
                else
                {
                    cr = new Wolf(this, randX, randY);
                }
                Predators.Add(cr);
                Field[randX, randY].Objects.Add(cr); 
            }
            
            for (var i = 0; i < AmountOmnivorous; i++)
            {
                int randX = Rnd.Next(1000), randY = Rnd.Next(1000);
                Mob<IOmnivorousFood> cr;
                if (i < AmountOmnivorous / 3)
                {
                    cr = new Bear(this, randX, randY);
                }
                else if (i < (AmountOmnivorous / 3)*2)
                {
                    cr = new Dog(this, randX, randY);
                }
                else
                {
                    cr = new Hedgehog(this, randX, randY);
                }
                Omnivorouses.Add(cr);
                Field[randX, randY].Objects.Add(cr); 
            }
            for (var i = 0; i < AmountHuman; i++)
            {
                int randX = Rnd.Next(1000), randY = Rnd.Next(1000);
                var cr = new Human(this, randX, randY);
                Humans.Add(cr);
                Field[randX, randY].Objects.Add(cr); 
            }

            for (var i = 0; i < AmountHouses; i++)
            {
                int randX = Rnd.Next(1000), randY = Rnd.Next(1000);
                var cr = new House(this, randX, randY);
                Houses.Add(cr);
                Field[randX, randY].Objects.Add(cr);
            }
            
            for (var i = 0; i < AmountGrass; i++)
            {
                int randX = Rnd.Next(1000), randY = Rnd.Next(1000);
                Grass cr;
                if (i < AmountGrass/3)
                {
                    cr = new Oats(this, randX, randY);
                }
                else if (i < (AmountGrass/3)*2)
                {
                    cr = new Carrot(this, randX, randY);
                }
                else
                {
                    cr = new GreenGrass(this, randX, randY);
                }
                AllGrass.Add(cr);
                Field[randX, randY].Objects.Add(cr);
            }
        }

        public bool CheckTheEnd(int x, int y, int a, int b)
        {
            return x + a < SizeOfGround &&
                   x + a >= 0 &&
                   y + b >= 0 &&
                   y + b < SizeOfGround;
        }
    }
}