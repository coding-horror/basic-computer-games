using System;
using System.Linq;

namespace Mugwump
{
    internal class Game
    {
        private readonly Grid _grid;

        private Game(Random random)
        {
            _grid = new Grid(Enumerable.Range(1, 4).Select(id => new Mugwump(id, random.Next(10), random.Next(10))));
        }

        public static void Play(Random random) => new Game(random).Play();

        private void Play()
        {
            for (int turn = 1; turn <= 10; turn++)
            {
                var guess = Input.ReadGuess($"Turn no. {turn} -- what is your guess");

                if (_grid.Check(guess))
                {
                    Console.WriteLine();
                    Console.WriteLine($"You got them all in {turn} turns!");
                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Sorry, that's 10 tries.  Here is where they're hiding:");
            _grid.Reveal();
        }
    }
}
