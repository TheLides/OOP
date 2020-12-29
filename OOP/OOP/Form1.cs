using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OOP.Classes;
using OOP.Classes.Creatures;
using OOP.Classes.Creatures.Plant;
using OOP.Classes.Creatures.Herbivore;
using OOP.Classes.Creatures.Omnivore;
using OOP.Classes.Creatures.Predators;

namespace OOP
{
    public partial class Form1 : Form
    {
        private Graphics _graphics;

        private const int Rows = 1000, Cols = 1000;
        private int _resolution;
        private readonly Ground _ground;
        private bool _madnessHasHappened;


        public Form1()
        {
            InitializeComponent();
            _ground = new Ground(Rows, Cols);
            _madnessHasHappened = false;
        }


        private void bStart_Click_1(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                return;

            _resolution = (int) Scale.Value;
            pictureBox2.Image = new Bitmap(Cols * _resolution, Rows * _resolution);
            _graphics = Graphics.FromImage(pictureBox2.Image);
            pictureBox2.Height *= _resolution;
            pictureBox2.Width *= _resolution;
            for (var x = 0; x < Rows; x++)
            {
                for (var y = 0; y < Cols; y++)
                {
                    if (_ground.Field[x, y].Objects.Any())
                    {
                        Brush color = GetSquareColor(x, y);
                        _graphics.FillRectangle(color, x * _resolution, y * _resolution,
                            _resolution, _resolution);
                    }
                    else
                    {
                        _graphics.FillRectangle(Brushes.White, x * _resolution, y * _resolution, _resolution,
                            _resolution);
                    }
                }
            }

            pictureBox2.Refresh();
            timer1.Start();
        }

        private void ShowInfo(string type, Creature clickedCreature)
        {
            label2.Text = $"Type: {type}\n" + $"Gender: {clickedCreature.GenderThis}\n" + $"X: {clickedCreature.X}\n" + $"Y: {clickedCreature.Y}\n" +
                          $"Starve: {clickedCreature.Starve}\n" + $"Wanna Child: {clickedCreature.WantChild}";
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.Location.X / _resolution, y = e.Location.Y / _resolution;
            if (e.Button != MouseButtons.Left) return;
            if (!_ground.Field[x, y].Objects.Any()) return;
            foreach (var c in _ground.Field[x, y].Objects)
            {
                if (c is GreenGrass)
                {
                    label2.Text = "Type: Green Grass";
                }

                if (c is Oats)
                {
                    label2.Text = "Type: Oats";
                }

                if (c is Carrot)
                {
                    label2.Text = "Type: Carrot";
                }

                if (c is Creature)
                {
                    var clickedCreature = c as Creature;
                    switch (clickedCreature)
                    {
                        case Deer _:
                            ShowInfo("Deer", clickedCreature);
                            break;
                        case Sheep _:
                            ShowInfo("Sheep", clickedCreature);
                            break;
                        case Rabbit _:
                            ShowInfo("Rabbit", clickedCreature);
                            break;
                        case Wolf _:
                            ShowInfo("Wolf", clickedCreature);
                            break;
                        case Wolverine _:
                            ShowInfo("Wolverine", clickedCreature);
                            break;
                        case Cat _:
                            ShowInfo("Cat", clickedCreature);
                            break;
                        case Bear _:
                            ShowInfo("Bear", clickedCreature);
                            break;
                        case Dog _:
                            ShowInfo("Dog", clickedCreature);
                            break;
                        case Hedgehog _:
                            ShowInfo("Hedgehog", clickedCreature);
                            break;
                        case Human _:
                            ShowInfo("Human", clickedCreature);
                            break;
                    }
                }
            }
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void bStop_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (!_madnessHasHappened)
            {
                PaintNextStep();
                if (_ground.Rnd.Next(100) == 1)
                {
                    Madness();
                }
            }
            else
            {
                PaintNextStep();
            }

            pictureBox2.Refresh();
        }


        private void Madness()
        {
            var x = _ground.Rnd.Next(Rows);
            var y = _ground.Rnd.Next(Cols);
            var length = _ground.Rnd.Next(750);
            for (var i = x; i < length + x && i < Rows; i++)
            {
                for (var j = y; j < length + y && j < Cols; j++)
                {
                    _ground.Field[i, j].IsAffectedByMadness = true;
                    _graphics.FillRectangle(Brushes.Crimson, i * _resolution,
                        j * _resolution, 1 * _resolution, 1 * _resolution);
                }
            }

            _madnessHasHappened = true;
        }

        private void PaintNextStep()
        {
            PaintGrass();
            PaintHerbivorous();
            PaintPredator();
            PaintOmnivorous();
            PaintHuman();
            PaintHouse();
        }

        private void PaintGrass()
        {
            for (var i = _ground.AllGrass.Count - 1; i >= 0; i--)
            {
                if (!_ground.AllGrass[i].Dead)
                {
                    _ground.AllGrass[i].Grow();
                    Brush color = GetSquareColor(_ground.AllGrass[i].X, _ground.AllGrass[i].Y);
                    _graphics.FillRectangle(color, _ground.AllGrass[i].X * _resolution,
                        _ground.AllGrass[i].Y * _resolution, 1 * _resolution, 1 * _resolution);
                }
                else
                {
                    _ground.Field[_ground.AllGrass[i].X, _ground.AllGrass[i].Y].Objects
                        .Remove(_ground.AllGrass[i]);
                    PaintCellMobDead(_ground.AllGrass[i]);
                    _ground.AllGrass.RemoveAt(i);
                }
            }
        }

        private void PaintHerbivorous()
        {
            for (var i = (_ground.Herbivorouses.Count - 1);
                i >= 0;
                i--)
            {
                var currentHerbivorous = _ground.Herbivorouses[i];
                if (!currentHerbivorous.Dead)
                {
                    var oldX = currentHerbivorous.X;
                    var oldY = currentHerbivorous.Y;
                    currentHerbivorous.Move();
                    PaintCellMobAlive(oldX, oldY, currentHerbivorous);
                }
                else
                {
                    _ground.Field[currentHerbivorous.X, currentHerbivorous.Y].Objects
                        .Remove(currentHerbivorous);
                    PaintCellMobDead(currentHerbivorous);
                    _ground.Herbivorouses.RemoveAt(i);
                }
            }
        }

        private void PaintPredator()
        {
            for (var i = (_ground.Predators.Count - 1);
                i >= 0;
                i--)
            {
                var currentPredator = _ground.Predators[i];
                if (!currentPredator.Dead)
                {
                    var oldX = currentPredator.X;
                    var oldY = currentPredator.Y;
                    currentPredator.Move();
                    PaintCellMobAlive(oldX, oldY, currentPredator);
                }
                else
                {
                    _ground.Field[currentPredator.X, currentPredator.Y].Objects
                        .Remove(currentPredator);
                    PaintCellMobDead(currentPredator);
                    _ground.Predators.RemoveAt(i);
                }
            }
        }

        private void PaintOmnivorous()
        {
            for (var i = (_ground.Omnivorouses.Count - 1);
                i >= 0;
                i--)
            {
                var currentOmnivorous = _ground.Omnivorouses[i];
                if (!currentOmnivorous.Dead)
                {
                    var oldX = currentOmnivorous.X;
                    var oldY = currentOmnivorous.Y;
                    currentOmnivorous.Move();
                    PaintCellMobAlive(oldX, oldY, currentOmnivorous);
                }
                else
                {
                    _ground.Field[currentOmnivorous.X, currentOmnivorous.Y].Objects
                        .Remove(currentOmnivorous);
                    PaintCellMobDead(currentOmnivorous);
                    _ground.Omnivorouses.RemoveAt(i);
                }
            }
        }

        private void PaintHuman()
        {
            for (var i = (_ground.Humans.Count - 1);
                i >= 0;
                i--)
            {
                var currentHuman = _ground.Humans[i];
                if (!currentHuman.Dead)
                {
                    var oldX = currentHuman.X;
                    var oldY = currentHuman.Y;
                    currentHuman.Move();
                    PaintCellMobAlive(oldX, oldY, currentHuman);
                }
                else
                {
                    _ground.Field[currentHuman.X, currentHuman.Y].Objects
                        .Remove(currentHuman);
                    PaintCellMobDead(currentHuman);
                    _ground.Humans.RemoveAt(i);
                }
            }
        }

        private void PaintHouse()
        {
            for (var i = (_ground.Houses.Count - 1);
                i >= 0;
                i--)
            {
                Brush color = GetSquareColor(_ground.Houses[i].X, _ground.Houses[i].Y);
                _graphics.FillRectangle(color, _ground.Houses[i].X * _resolution,
                    _ground.Houses[i].Y * _resolution, 1 * _resolution, 1 * _resolution);
            }
        }

        private void PaintCellMobAlive(int oldX, int oldY, Creature cr)
        {
            Brush newColor = GetSquareColor(cr.X, cr.Y);
            Brush color = GetSquareColor(oldX, oldY);
            _graphics.FillRectangle(color, oldX * _resolution,
                oldY * _resolution, 1 * _resolution, 1 * _resolution);
            _graphics.FillRectangle(newColor, cr.X * _resolution,
                cr.Y * _resolution, 1 * _resolution, 1 * _resolution);
        }

        private void PaintCellMobDead(Creature cr)
        {
            if (_ground.Field[cr.X, cr.Y].IsAffectedByMadness)
            {
                _graphics.FillRectangle(Brushes.Crimson, cr.X * _resolution,
                    cr.Y * _resolution, 1 * _resolution, 1 * _resolution);
            }
            else
            {
                _graphics.FillRectangle(Brushes.White, cr.X * _resolution,
                    cr.Y * _resolution, 1 * _resolution, 1 * _resolution);
            }
        }

        private Brush GetSquareColor(int x, int y)
        {
            var square = _ground.Field[x, y];
            if (square.Objects.Any(c => c is House))
            {
                return Brushes.Teal;
            }
            if (square.Objects.Any(c => c is Human))
            {
                return Brushes.Black;
            }

            if (square.Objects.Any(c => c is Bear))
            {
                return Brushes.DarkRed;
            }

            if (square.Objects.Any(c => c is Dog))
            {
                return Brushes.Chocolate;
            }

            if (square.Objects.Any(c => c is Hedgehog))
            {
                return Brushes.LightSeaGreen;
            }

            if (square.Objects.Any(c => c is Wolf))
            {
                return Brushes.Gray;
            }

            if (square.Objects.Any(c => c is Wolverine))
            {
                return Brushes.Maroon;
            }

            if (square.Objects.Any(c => c is Cat))
            {
                return Brushes.BlueViolet;
            }

            if (square.Objects.Any(c => c is Deer))
            {
                return Brushes.SandyBrown;
            }

            if (square.Objects.Any(c => c is Sheep))
            {
                return Brushes.DimGray;
            }

            if (square.Objects.Any(c => c is Rabbit))
            {
                return Brushes.Sienna;
            }

            if (square.Objects.Any(c => c is Carrot))
            {
                return Brushes.Orange;
            }

            if (square.Objects.Any(c => c is Oats))
            {
                return Brushes.Goldenrod;
            }

            if (square.Objects.Any(c => c is GreenGrass))
            {
                return Brushes.ForestGreen;
            }

            if (square.IsAffectedByMadness)
            {
                return Brushes.Crimson;
            }

            return Brushes.White;
        }
    }
}