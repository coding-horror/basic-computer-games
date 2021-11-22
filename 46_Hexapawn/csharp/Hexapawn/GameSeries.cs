using System;

namespace Hexapawn
{
    // Runs series of games between the computer and the human player
    internal class GameSeries
    {
        private readonly Computer _computer = new();
        private readonly Human _human = new();

        public void Play()
        {
            while (true)
            {
                var game = new Game(_human, _computer);

                var winner = game.Play();
                winner.AddWin();
                Console.WriteLine(winner == _computer ? "I win." : "You win.");

                Console.Write($"I have won {_computer.Wins} and you {_human.Wins}");
                Console.WriteLine($" out of {_computer.Wins + _human.Wins} games.");
                Console.WriteLine();
            }
        }
    }
}
