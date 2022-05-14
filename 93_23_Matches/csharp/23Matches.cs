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
        Game();
      }
      else
      {
        Console.WriteLine("HEADS! I WIN! HA! HA!");
        Console.WriteLine("PREPARE TO LOSE, MEATBALL-NOSE!!");
        Console.Write("\n");
        Console.WriteLine("I TAKE 2 MATCHES");
        numberOfMatches = numberOfMatches - 2;
        Game();
      }

    }

    static void Game()
    {

    }

  }
}