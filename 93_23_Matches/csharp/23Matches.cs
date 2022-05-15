using System;

namespace Program
{
  class Program
  {
    static void Main(string[] args)
    {
      // Print introduction text

      // Prints the title with 31 spaces placed in front of the text using the PadLeft() string function
      Console.WriteLine("23 MATCHES".PadLeft(31));
      Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".PadLeft(15));
      
      // Print 3 blank lines with \n escape sequence
      Console.Write("\n\n\n");
      Console.WriteLine(" THIS IS A GAME CALLED '23 MATCHES'.");
      Console.Write("\n");

      Console.WriteLine("WHEN IT IS YOUR TURN, YOU MAY TAKE ONE, TWO, OR THREE");
      Console.WriteLine("MATCHES. THE OBJECT OF THE GAME IS NOT TO HAVE TO TAKE");
      Console.WriteLine("THE LAST MATCH.");
      Console.Write("\n");
      Console.WriteLine("LET'S FLIP A COIN TO SEE WHO GOES FIRST.");
      Console.WriteLine("IF IT COMES UP HEADS, I WILL WIN THE TOSS.");
      Console.Write("\n");

      
      int numberOfMatches = 23;

      // Create a random class object to generate the coin toss
      Random random = new Random();
      // Generates a random number between 0.0 and 1.0
      // Multiplies that number by 2 and then
      // Converts it into an integer giving either a 0 or a 1
      int coinTossResult = (int)(2 * random.NextDouble()); 

      if (coinTossResult == 1)
      {
        Console.WriteLine("TAILS! YOU GO FIRST. ");
        Console.Write("\n");
        numberOfMatches = PlayerTurn(numberOfMatches)[0];
      }
      else
      {
        Console.WriteLine("HEADS! I WIN! HA! HA!");
        Console.WriteLine("PREPARE TO LOSE, MEATBALL-NOSE!!");
        Console.Write("\n");
        Console.WriteLine("I TAKE 2 MATCHES");
        numberOfMatches = numberOfMatches - 2;
      }

      do
      {
        Console.WriteLine("THE NUMBER OF MATCHES IS NOW " + numberOfMatches);
        int[] totalNumberAndNumberRemoved = PlayerTurn(numberOfMatches);
        numberOfMatches = ComputerTurn(totalNumberAndNumberRemoved);        
      } while (numberOfMatches > 1);

    }


    static int[] PlayerTurn(int numberOfMatches)
    {
      Console.WriteLine("YOUR TURN -- YOU MAY TAKE 1, 2, OR 3 MATCHES.");
      Console.Write("HOW MANY DO YOU WISH TO REMOVE ?? ");
      // Get player input
      int numberOfMatchesRemove = ReadPlayerInput();
      // If the input is invalid (not 1, 2, or 3)
      // then ask the player to input again
      while (numberOfMatchesRemove > 3 || numberOfMatchesRemove <= 0)
      {
        Console.WriteLine("VERY FUNNY! DUMMY!");
        Console.WriteLine("DO YOU WANT TO PLAY OR GOOF AROUND?");
        Console.Write("NOW, HOW MANY MATCHES DO YOU WANT                 ??");
        numberOfMatchesRemove = ReadPlayerInput();
      }

      // Remove the player specified number of matches
      numberOfMatches = numberOfMatches - numberOfMatchesRemove;

      Console.WriteLine("THE ARE NOW " + numberOfMatches + " MATCHES REMAINING");
      int[] totalNumberAndNumberRemoved = {numberOfMatches, numberOfMatchesRemove};
      return totalNumberAndNumberRemoved;
      
    }
    static int ComputerTurn(int[] totalNumberAndNumberRemoved)
    {
      int numberOfMatches = totalNumberAndNumberRemoved[0];
      int numberOfMatchesRemovedByPlayer = totalNumberAndNumberRemoved[1];

      int numberOfMatchesRemovedByComputer = 0;
      switch (numberOfMatches)
      {
        case 4:
          numberOfMatchesRemovedByComputer = 3;
          break;
        case 3:
          numberOfMatchesRemovedByComputer = 2;
          break;
        case 2:
          numberOfMatchesRemovedByComputer = 1;
          break;
        case 1: case 0:
          Console.WriteLine("YOU WON, FLOPPY EARS !");
          Console.WriteLine("THING YOU'RE PRETTY SMART !");
          Console.WriteLine("LETS PLAY AGAIN AND I'LL BLOW YOUR SHOES OFF !!");
          break;
        default:
          numberOfMatchesRemovedByComputer = 4 - numberOfMatchesRemovedByPlayer;
          break;
      }

      if (numberOfMatchesRemovedByComputer != 0)
      {
        Console.WriteLine("MY TURN ! I REMOVE " + numberOfMatchesRemovedByComputer + " MATCHES");
        if (numberOfMatches <= 1)
        {
          Console.Write("\n");
          Console.WriteLine("YOU POOR BOOB! YOU TOOK THE LAST MATCH! I GOTCHA!!");
          Console.WriteLine("HA ! HA ! I BEAT YOU !!!");
          Console.Write("\n");
          Console.WriteLine("GOOD BYE LOSER!");
        }
      }



      
      return numberOfMatches;
    }

    static int ReadPlayerInput()
    {
      // Read user input and convert to integer
      int playerInput = Convert.ToInt32(Console.ReadLine());
      return playerInput;      
    }

  }
}