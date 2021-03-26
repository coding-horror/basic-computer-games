using System;



namespace War
{
    // This class displays all the text that the user sees when playing the game.
    // It also handles asking the user a yes/no question and returning their answer.
    public class UserInterface
    {
        public void WriteIntro()
        {
            Console.WriteLine("                                 WAR");
            Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();

            Console.WriteLine("THIS IS THE CARD GAME OF WAR.  EACH CARD IS GIVEN BY SUIT-#");
            Console.Write("AS S-7 FOR SPADE 7.  ");

            if (AskAQuestion("DO YOU WANT DIRECTIONS? "))
            {
                Console.WriteLine("THE COMPUTER GIVES YOU AND IT A 'CARD'.  THE HIGHER CARD");
                Console.WriteLine("(NUMERICALLY) WINS.  THE GAME ENDS WHEN YOU CHOOSE NOT TO");
                Console.WriteLine("CONTINUE OR WHEN YOU HAVE FINISHED THE PACK.");
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public void WriteAResult(Card yourCard, Card computersCard, ref int computersScore, ref int yourScore)
        {
            Console.WriteLine($"YOU: {yourCard}     COMPUTER: {computersCard}");
            if (yourCard < computersCard)
            {
                computersScore++;
                Console.WriteLine($"THE COMPUTER WINS!!! YOU HAVE {yourScore} AND THE COMPUTER HAS {computersScore}");
            }
            else if (yourCard > computersCard)
            {
                yourScore++;
                Console.WriteLine($"YOU WIN. YOU HAVE {yourScore} AND THE COMPUTER HAS {computersScore}");
            }
            else
            {
                Console.WriteLine("TIE.  NO SCORE CHANGE");
            }
        }

        public bool AskAQuestion(string question)
        {
            // Repeat asking the question until the user answers "YES" or "NO".
            while (true)
            {
                Console.Write(question);
                string result = Console.ReadLine();

                if (result.ToLower() == "yes")
                {
                    Console.WriteLine();
                    return true;
                }
                else if (result.ToLower() == "no")
                {
                    Console.WriteLine();
                    return false;
                }

                Console.WriteLine("YES OR NO, PLEASE.");
            }
        }

        public void WriteClosingRemarks(bool usedAllCards, int yourScore, int computersScore)
        {
            if (usedAllCards)
            {
                Console.WriteLine("WE HAVE RUN OUT OF CARDS.");
            }
            Console.WriteLine($"FINAL SCORE:  YOU: {yourScore}  THE COMPUTER: {computersScore}");
            Console.WriteLine("THANKS FOR PLAYING.  IT WAS FUN.");
        }
    }
}
