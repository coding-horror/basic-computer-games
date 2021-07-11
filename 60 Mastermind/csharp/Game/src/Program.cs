using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    // MASTERMIND II
    // STEVE NORTH
    // CREATIVE COMPUTING
    // PO BOX 789-M MORRISTOWN NEW JERSEY 07960
    class Program
    {
        public const int MaximumGuesses = 10;

        static void Main()
        {
            var (codeFactory, rounds) = StartGame();

            var random        = new Random();
            var humanScore    = 0;
            var computerScore = 0;

            for (var round = 1; round <= rounds; ++round)
            {
                View.ShowStartOfRound(round);

                if (!HumanTakesTurn())
                    return;

                while (!ComputerTakesTurn())
                    View.ShowInconsistentInformation();
            }

            View.ShowScores(humanScore, computerScore, isFinal: true);

            /// <summary>
            /// Gets the game start parameters from the user.
            /// </summary>
            (CodeFactory codeFactory, int rounds) StartGame()
            {
                View.ShowBanner();

                var colors    = Controller.GetNumberOfColors();
                var positions = Controller.GetNumberOfPositions();
                var rounds    = Controller.GetNumberOfRounds();

                var codeFactory = new CodeFactory(positions, colors);

                View.ShowTotalPossibilities(codeFactory.Possibilities);
                View.ShowColorTable(codeFactory.Colors);

                return (codeFactory, rounds);
            }

            /// <summary>
            /// Executes the human's turn.
            /// </summary>
            /// <returns>
            /// True if thue human completed his or her turn and false if
            /// he or she quit the game.
            /// </returns>
            bool HumanTakesTurn()
            {
                // Store a history of the human's guesses (used for the show
                // board command below).
                var history     = new List<TurnResult>();
                var code        = codeFactory.Create(random);
                var guessNumber = default(int);

                for (guessNumber = 1; guessNumber <= MaximumGuesses; ++guessNumber)
                {
                    var guess = default(Code);

                    while (guess is null)
                    {
                        switch (Controller.GetCommand(guessNumber, codeFactory.Positions, codeFactory.Colors))
                        {
                            case (Command.MakeGuess, Code input):
                                guess = input;
                                break;
                            case (Command.ShowBoard, _):
                                View.ShowBoard(history);
                                break;
                            case (Command.Quit, _):
                                View.ShowQuitGame(code);
                                return false;
                        }
                    }

                    var (blacks, whites) = code.Compare(guess);
                    if (blacks == codeFactory.Positions)
                        break;

                    View.ShowResults(blacks, whites);

                    history.Add(new TurnResult(guess, blacks, whites));
                }

                if (guessNumber <= MaximumGuesses)
                    View.ShowHumanGuessedCode(guessNumber);
                else
                    View.ShowHumanFailedToGuessCode(code);

                humanScore += guessNumber;

                View.ShowScores(humanScore, computerScore, isFinal: false);
                return true;
            }

            /// <summary>
            /// Executes the computers turn.
            /// </summary>
            /// <returns>
            /// True if the computer completes its turn successfully and false
            /// if it does not (due to human error).
            /// </returns>
            bool ComputerTakesTurn()
            {
                var isCandidate = new bool[codeFactory.Possibilities];
                var guessNumber = default(int);

                Array.Fill(isCandidate, true);

                View.ShowComputerStartTurn();
                Controller.WaitUntilReady();

                for (guessNumber = 1; guessNumber <= MaximumGuesses; ++guessNumber)
                {
                    // Starting with a random code, cycle through codes until
                    // we find one that is still a candidate solution.  If
                    // there are no remaining candidates, then it implies that
                    // the user made an error in one or more responses.
                    var codeNumber = EnumerableExtensions.Cycle(random.Next(codeFactory.Possibilities), codeFactory.Possibilities)
                        .FirstOrDefault(i => isCandidate[i], -1);

                    if (codeNumber < 0)
                        return false;

                    var guess = codeFactory.Create(codeNumber);

                    var (blacks, whites) = Controller.GetBlacksWhites(guess);
                    if (blacks == codeFactory.Positions)
                        break;

                    // Mark codes which are no longer potential solutions.  We
                    // know that the current guess yields the above number of
                    // blacks and whites when compared to the solution, so any
                    // code that yields a different number of blacks or whites
                    // can't be the answer.
                    foreach (var (candidate, index) in codeFactory.EnumerateCodes().Select((candidate, index) => (candidate, index)))
                    {
                        if (isCandidate[index])
                        {
                            var (candidateBlacks, candidateWhites) = guess.Compare(candidate);
                            if (blacks != candidateBlacks || whites != candidateWhites)
                                isCandidate[index] = false;
                        }
                    }
                }

                if (guessNumber <= MaximumGuesses)
                    View.ShowComputerGuessedCode(guessNumber);
                else
                    View.ShowComputerFailedToGuessCode();

                computerScore += guessNumber;
                View.ShowScores(humanScore, computerScore, isFinal: false);

                return true;
            }
        }
    }
}
