using System;

namespace Program
{
  class Program
  {

    // Initialize 3 public variables so that they can be ascessed anywhere in the code
    public static int numberOfMatches;
    public static int numberOfMatchesRemovedByPlayer;
    public static bool playerGoesFirst = false; // a flag to show if the player won the coin toss
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

      // Set the number of matches to 23
      numberOfMatches = 23;

      // Create a random class object to generate the coin toss
      Random random = new Random();
      // Generates a random number between 0.0 and 1.0
      // Multiplies that number by 2 and then
      // Converts it into an integer giving either a 0 or a 1
      int coinTossResult = (int)(2 * random.NextDouble()); 

      if (coinTossResult == 1)
      {
        Console.WriteLine("TAILS! YOU GO FIRST. ");
        // Sets the player coin toss flag to true
        playerGoesFirst = true;
        PlayerTurn();
      }
      else
      {
        Console.WriteLine("HEADS! I WIN! HA! HA!");
        Console.WriteLine("PREPARE TO LOSE, MEATBALL-NOSE!!");
        Console.Write("\n");
        Console.WriteLine("I TAKE 2 MATCHES");
        numberOfMatches = numberOfMatches - 2;
      }

      // loops the code until there is 1 or fewer matches
      do
      {
        // Checks if the player has already gone 
        // because they won the coin toss
        // if they have not then the player can go
        if (playerGoesFirst == false)
        {
          Console.Write("THE NUMBER OF MATCHES IS NOW " + numberOfMatches);
          PlayerTurn();
        }
        // sets the coint toss flag to false since
        // this is only needed on the first loop of the code
        playerGoesFirst = false;
        ComputerTurn();        
      } while (numberOfMatches > 1);

    }

    static void PlayerTurn()
    {
      Console.WriteLine("\n");
      Console.WriteLine("YOUR TURN -- YOU MAY TAKE 1, 2, OR 3 MATCHES.");
      Console.Write("HOW MANY DO YOU WISH TO REMOVE ?? ");
      // Get player input
      numberOfMatchesRemovedByPlayer = ReadPlayerInput();
      // If the input is invalid (not 1, 2, or 3)
      // then ask the player to input again
      while (numberOfMatchesRemovedByPlayer > 3 || numberOfMatchesRemovedByPlayer <= 0)
      {
        Console.WriteLine("VERY FUNNY! DUMMY!");
        Console.WriteLine("DO YOU WANT TO PLAY OR GOOF AROUND?");
        Console.Write("NOW, HOW MANY MATCHES DO YOU WANT                 ??");
        numberOfMatchesRemovedByPlayer = ReadPlayerInput();
      }

      // Remove the player specified number of matches
      numberOfMatches = numberOfMatches - numberOfMatchesRemovedByPlayer;

      Console.WriteLine("THE ARE NOW " + numberOfMatches + " MATCHES REMAINING");      
    }
    static void ComputerTurn()
    {
      // Initialize the numberOfMatchesRemovedByComputer
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
        case 1: case 0: // If the computer losses call this case
          Console.WriteLine("YOU WON, FLOPPY EARS !");
          Console.WriteLine("THING YOU'RE PRETTY SMART !");
          Console.WriteLine("LETS PLAY AGAIN AND I'LL BLOW YOUR SHOES OFF !!");
          break;
        default: // If there are > than 4 matches call this case
          numberOfMatchesRemovedByComputer = 4 - numberOfMatchesRemovedByPlayer;
          break;
      }
      // If the numberOfMatchesRemovedByComputer has been updated run this code,
      // if not them the computer has lost
      if (numberOfMatchesRemovedByComputer != 0)
      {
        Console.WriteLine("MY TURN ! I REMOVE " + numberOfMatchesRemovedByComputer + " MATCHES");
        numberOfMatches = numberOfMatches - numberOfMatchesRemovedByComputer;
        // If there are less than or equal to 1 matches
        // then the player has lost        
        if (numberOfMatches <= 1)
        {
          Console.Write("\n");
          Console.WriteLine("YOU POOR BOOB! YOU TOOK THE LAST MATCH! I GOTCHA!!");
          Console.WriteLine("HA ! HA ! I BEAT YOU !!!");
          Console.Write("\n");
          Console.WriteLine("GOOD BYE LOSER!");
        }
      }
    }


    // This method handles the player input 
    // and will handle inncorrect input
    static int ReadPlayerInput()
    {
      // Read user input and convert to integer
      int playerInput = 0;
      // Try to read player input
      try
      {
        playerInput = Convert.ToInt32(Console.ReadLine());
      }
      // If there is an error in the player input
      catch (System.Exception)
      {
        Console.WriteLine("?REENTER");
        Console.Write("?? ");
        // Ask the player to reenter their input
        playerInput = ReadPlayerInput();
      }
      return playerInput;      
    }

  }
}