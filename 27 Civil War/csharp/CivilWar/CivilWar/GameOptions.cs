using System;

using static CivilWar.ConsoleUtils;

namespace CivilWar
{
    public record GameOptions(bool TwoPlayers, bool ShowDescriptions)
    {
        public static GameOptions Input()
        {
            Console.WriteLine(
@"                          Civil War
               Creative Computing, Morristown, New Jersey


Do you want instructions?");

            const string instructions = @"This is a civil war simulation.
To play type a response when the computer asks.
Remember that all factors are interrelated and that your responses could change history. Facts and figures used are based on the actual occurrence. Most battles tend to result as they did in the civil war, but it all depends on you!!

The object of the game is to win as many battles as possible.

Your choices for defensive strategy are:
        (1) artillery attack
        (2) fortification against frontal attack
        (3) fortification against flanking maneuvers
        (4) falling back
Your choices for offensive strategy are:
        (1) artillery attack
        (2) frontal attack
        (3) flanking maneuvers
        (4) encirclement
You may surrender by typing a '5' for your strategy.";

            if (InputYesOrNo())
                WriteWordWrap(instructions);

            Console.WriteLine("\n\nAre there two generals present?");
            bool twoPlayers = InputYesOrNo();
            if (!twoPlayers)
                Console.WriteLine("\nYou are the confederacy.  Good luck!\n");

            WriteWordWrap(
            @"Select a battle by typing a number from 1 to 14 on request.  Type any other number to end the simulation. But '0' brings back exact previous battle situation allowing you to replay it.

Note: a negative Food$ entry causes the program to use the entries from the previous battle

After requesting a battle, do you wish battle descriptions (answer yes or no)");
            bool showDescriptions = InputYesOrNo();

            return new GameOptions(twoPlayers, showDescriptions);
        }
    }
}
