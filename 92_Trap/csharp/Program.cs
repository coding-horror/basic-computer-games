using System;

namespace trap_cs
{
  class Program
  {
    const int maxGuesses = 6;
    const int maxNumber = 100;
    static void Main(string[] args)
    {
      int lowGuess  = 0;
      int highGuess = 0;

      Random randomNumberGenerator = new ();

      Print("TRAP");
      Print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
      Print();
      Print();
      Print();

      PrintInstructions();

      int numberToGuess = randomNumberGenerator.Next(1, maxNumber);

      for (int nGuess = 1; nGuess <= maxGuesses + 1; nGuess++)
      {
        if (nGuess > maxGuesses)
        {
          Print(string.Format("SORRY, THAT'S {0} GUESSES. THE NUMBER WAS {1}", maxGuesses, numberToGuess));
          Print();
          break;
        }

        GetGuesses(nGuess, ref lowGuess, ref highGuess);

        if(lowGuess == highGuess && lowGuess == numberToGuess)
        {
          Print("YOU GOT IT!!!");
          Print();
          Print("TRY AGAIN.");
          Print();
          break;
        }
        if (highGuess < numberToGuess)
        {
          Print("MY NUMBER IS LARGER THAN YOUR TRAP NUMBERS.");
        }
        else if (lowGuess > numberToGuess)
        {
          Print("MY NUMBER IS SMALLER THAN YOUR TRAP NUMBERS.");
        }
        else
        {
          Print("YOU HAVE TRAPPED MY NUMBER.");
        }
      }
    }

// TRAP
// REM - STEVE ULLMAN, 8 - 1 - 72
    static void PrintInstructions()
    {
      Print("INSTRUCTIONS ?");

      char response = Console.ReadKey().KeyChar;
      if (response == 'Y')
      {
        Print(string.Format("I AM THINKING OF A NUMBER BETWEEN 1 AND {0}", maxNumber));
        Print("TRY TO GUESS MY NUMBER. ON EACH GUESS,");
        Print("YOU ARE TO ENTER 2 NUMBERS, TRYING TO TRAP");
        Print("MY NUMBER BETWEEN THE TWO NUMBERS. I WILL");
        Print("TELL YOU IF YOU HAVE TRAPPED MY NUMBER, IF MY");
        Print("NUMBER IS LARGER THAN YOUR TWO NUMBERS, OR IF");
        Print("MY NUMBER IS SMALLER THAN YOUR TWO NUMBERS.");
        Print("IF YOU WANT TO GUESS ONE SINGLE NUMBER, TYPE");
        Print("YOUR GUESS FOR BOTH YOUR TRAP NUMBERS.");
        Print(string.Format("YOU GET {0} GUESSES TO GET MY NUMBER.", maxGuesses));
      }
    }
    static void Print(string stringToPrint)
    { 
      Console.WriteLine(stringToPrint);
    }
    static void Print()
    {
      Console.WriteLine();
    }
    static void GetGuesses(int nGuess, ref int lowGuess, ref int highGuess)
    {
      Print();
      Print(string.Format("GUESS #{0}", nGuess));

      lowGuess  = GetIntFromConsole("Type low guess");
      highGuess = GetIntFromConsole("Type high guess");

      if(lowGuess > highGuess)
      {
        int tempGuess = lowGuess;

        lowGuess = highGuess;
        highGuess = tempGuess;
      }
    }
    static int GetIntFromConsole(string prompt)
    {

      Console.Write( prompt + " > ");
      string intAsString = Console.ReadLine();

      if(int.TryParse(intAsString, out int intValue) ==false)
      {
        intValue = 1;
      }

      return intValue;
    }
  }
}
