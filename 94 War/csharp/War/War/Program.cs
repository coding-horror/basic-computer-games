using System;

namespace War
{
	public class Intro
	{
		public void WriteIntro()
		{
			Console.WriteLine("                                 WAR");
			Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
	
			Console.WriteLine("THIS IS THE CARD GAME OF WAR.  EACH CARD IS GIVEN BY SUIT-#");
			Console.WriteLine("AS S-7 FOR SPADE 7.  ");
	
			if (AskQuestion("DO YOU WANT DIRECTIONS? "))
			{
				Console.WriteLine("THE COMPUTER GIVES YOU AND IT A 'CARD'.  THE HIGHER CARD");
				Console.WriteLine("(NUMERICALLY) WINS.  THE GAME ENDS WHEN YOU CHOOSE NOT TO");
				Console.WriteLine("CONTINUE OR WHEN YOU HAVE FINISHED THE PACK.");
			}
	
			Console.WriteLine();
			Console.WriteLine();
		}
	
		public bool AskQuestion(string question)
		{
			while (true)
			{
				Console.Write(question);
				string result = Console.ReadLine();
	
				if (result.ToLower()[0] == 'y')
				{
					return true;
				}
				else /*if (result.ToLower() == "no")*/
				{
					return false;
				}
	
				Console.WriteLine("YES OR NO, PLEASE.");
			}
		}
    }

    class Program
    {
        static void Main(string[] args)
        {
            var intro = new Intro();
			intro.WriteIntro();

			var deck = new Deck();
			deck.Shuffle();

			int yourScore = 0;
            int computersScore = 0;
			bool usedAllCards = true;

            for (int i = 0; i < Deck.deckSize; i += 2)
            {
				var yourCard = deck.GetCard(i);
                var computersCard = deck.GetCard(i + 1);

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

				if (!intro.AskQuestion("DO YOU WANT TO CONTINUE? "))
                {
					usedAllCards = false;
					break;
                }
            }

			if (usedAllCards)
            {
				Console.WriteLine("WE HAVE RUN OUT OF CARDS.");
            }
			Console.WriteLine($"FINAL SCORE:  YOU: {yourScore}  THE COMPUTER: {computersScore}");
			Console.WriteLine("THANKS FOR PLAYING.  IT WAS FUN.");
        }
    }
}
