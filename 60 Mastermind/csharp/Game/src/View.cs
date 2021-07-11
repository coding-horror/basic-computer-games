using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    /// <summary>
    /// Contains functions for displaying information to the end user.
    /// </summary>
    public static class View
    {
        public static void ShowBanner()
        {
            Console.WriteLine("                              MASTERMIND");
            Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ShowTotalPossibilities(int possibilities)
        {
            Console.WriteLine($"TOTAL POSSIBILITIES = {possibilities}");
            Console.WriteLine();
        }

        public static void ShowColorTable(int numberOfColors)
        {
            Console.WriteLine();
            Console.WriteLine("COLOR     LETTER");
            Console.WriteLine("=====     ======");

            foreach (var color in Colors.List.Take(numberOfColors))
                Console.WriteLine($"{color.LongName,-13}{color.ShortName}");

            Console.WriteLine();
        }

        public static void ShowStartOfRound(int roundNumber)
        {
            Console.WriteLine();
            Console.WriteLine($"ROUND NUMBER {roundNumber} ----");
            Console.WriteLine();
            Console.WriteLine("GUESS MY COMBINATION.");
            Console.WriteLine();
        }

        public static void ShowBoard(IEnumerable<TurnResult> history)
        {
            Console.WriteLine();
            Console.WriteLine("BOARD");
            Console.WriteLine("MOVE     GUESS          BLACK     WHITE");

            var moveNumber = 0;
            foreach (var result in history)
                Console.WriteLine($"{++moveNumber,-9}{result.Guess,-16}{result.Blacks,-10}{result.Whites}");

            Console.WriteLine();
        }

        public static void ShowQuitGame(Code code)
        {
            Console.WriteLine($"QUITTER!  MY COMBINATION WAS: {code}");
            Console.WriteLine("GOOD BYE");
        }

        public static void ShowResults(int blacks, int whites)
        {
            Console.WriteLine($"YOU HAVE  {blacks}  BLACKS AND  {whites}  WHITES.");
        }

        public static void ShowHumanGuessedCode(int guessNumber)
        {
            Console.WriteLine($"YOU GUESSED IT IN  {guessNumber}  MOVES!");
        }

        public static void ShowHumanFailedToGuessCode(Code code)
        {
            // Note: The original code did not print out the combination, but
            // this appears to be a bug.
            Console.WriteLine("YOU RAN OUT OF MOVES!  THAT'S ALL YOU GET!");
            Console.WriteLine($"THE ACTUAL COMBINATION WAS: {code}");
        }

        public static void ShowScores(int humanScore, int computerScore, bool isFinal)
        {
            if (isFinal)
            {
                Console.WriteLine("GAME OVER");
                Console.WriteLine("FINAL SCORE:");
            }
            else
                Console.WriteLine("SCORE:");

            Console.WriteLine($"     COMPUTER  {computerScore}");
            Console.WriteLine($"     HUMAN     {humanScore}");
            Console.WriteLine();
        }

        public static void ShowComputerStartTurn()
        {
            Console.WriteLine("NOW I GUESS.  THINK OF A COMBINATION.");
        }

        public static void ShowInconsistentInformation()
        {
            Console.WriteLine("YOU HAVE GIVEN ME INCONSISTENT INFORMATION.");
            Console.WriteLine("TRY AGAIN, AND THIS TIME PLEASE BE MORE CAREFUL.");
        }

        public static void ShowComputerGuessedCode(int guessNumber)
        {
            Console.WriteLine($"I GOT IT IN  {guessNumber}  MOVES!");
        }

        public static void ShowComputerFailedToGuessCode()
        {
            Console.WriteLine("I USED UP ALL MY MOVES!");
            Console.WriteLine("I GUESS MY CPU IS JUST HAVING AN OFF DAY.");
        }

        public static void PromptNumberOfColors()
        {
            Console.Write("NUMBER OF COLORS? ");
        }

        public static void PromptNumberOfPositions()
        {
            Console.Write("NUMBER OF POSITIONS? ");
        }

        public static void PromptNumberOfRounds()
        {
            Console.Write("NUMBER OF ROUNDS? ");
        }

        public static void PromptGuess(int moveNumber)
        {
            Console.Write($"MOVE #  {moveNumber}  GUESS ? ");
        }

        public static void PromptReady()
        {
            Console.Write("HIT RETURN WHEN READY ? ");
        }

        public static void PromptBlacksWhites(Code code)
        {
            Console.Write($"MY GUESS IS: {code}");
            Console.Write("  BLACKS, WHITES ? ");
        }

        public static void PromptTwoValues()
        {
            Console.WriteLine("PLEASE ENTER TWO VALUES, SEPARATED BY A COMMA");
        }

        public static void PromptValidInteger()
        {
            Console.WriteLine("PLEASE ENTER AN INTEGER VALUE");
        }

        public static void NotifyBadNumberOfPositions()
        {
            Console.WriteLine("BAD NUMBER OF POSITIONS");
        }

        public static void NotifyInvalidColor(char colorKey)
        {
            Console.WriteLine($"'{colorKey}' IS UNRECOGNIZED.");
        }

        public static void NotifyTooManyColors(int maxColors)
        {
            Console.WriteLine($"NO MORE THAN {maxColors}, PLEASE!");
        }
    }
}
