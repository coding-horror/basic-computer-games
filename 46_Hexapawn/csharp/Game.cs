using System;

namespace Hexapawn
{
    // Runs a single game of Hexapawn
    internal class Game
    {
        private readonly Board _board;
        private readonly Human _human;
        private readonly Computer _computer;

        public Game(Human human, Computer computer)
        {
            _board = new Board();
            _human = human;
            _computer = computer;
        }

        public IPlayer Play()
        {
            Console.WriteLine(_board);

            while(true)
            {
                _human.Move(_board);

                Console.WriteLine(_board);

                if (!_computer.TryMove(_board))
                {
                    return _human;
                }

                Console.WriteLine(_board);

                if (_computer.IsFullyAdvanced(_board) || _human.HasNoPawns(_board))
                {
                    return _computer;
                }

                if (!_human.HasLegalMove(_board))
                {
                    Console.Write("You can't move, so ");
                    return _computer;
                }
            }
        }
    }
}
