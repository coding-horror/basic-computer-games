using System;
using System.Linq;

namespace RockScissorsPaper
{
    public class Game
    {
        public int ComputerWins { get; private set; }
        public int HumanWins { get; private set; }
        public int TieGames { get; private set; }

        public void PlayGame()
        {
            var computerChoice = Choices.GetRandom();
            var humanChoice = GetHumanChoice();

            Console.WriteLine("This is my choice...");
            Console.WriteLine("...{0}", computerChoice.Name);

            if (humanChoice.Beats(computerChoice))
            {
                Console.WriteLine("You win!!!");
                HumanWins++;
            }
            else if (computerChoice.Beats(humanChoice))
            {
                Console.WriteLine("Wow!  I win!!!");
                ComputerWins++;
            }
            else
            {
                Console.WriteLine("Tie game.  No winner.");
                TieGames++;
            }
        }

        public void WriteFinalScore()
        {
            Console.WriteLine();
            Console.WriteLine("Here is the final game score:");
            Console.WriteLine("I have won {0} game(s).", ComputerWins);
            Console.WriteLine("You have one {0} game(s).", HumanWins);
            Console.WriteLine("And {0} game(s) ended in a tie.", TieGames);
        }

        public Choice GetHumanChoice()
        {
            while (true)
            {
                Console.WriteLine("3=Rock...2=Scissors...1=Paper");
                Console.WriteLine("1...2...3...What's your choice");
                if (Choices.TryGetBySelector(Console.ReadLine(), out var choice))
                    return choice;
                Console.WriteLine("Invalid.");
            }
        }
    }
}
