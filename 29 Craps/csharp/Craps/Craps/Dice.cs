using System;


namespace Craps
{
    public class Dice
    {
        private Random rand = new Random();
        public readonly int sides;

        public Dice()
        {
            sides = 6;
        }

        public Dice(int sides)
        {
            this.sides = sides;
        }

        public int Roll() => rand.Next(1, sides + 1);
    }
}
