// SUPER STARTREK - MAY 16,1978 - REQUIRES 24K MEMORY
//
// ****         **** STAR TREK ****        ****
// ****  SIMULATION OF A MISSION OF THE STARSHIP ENTERPRISE,
// ****  AS SEEN ON THE STAR TREK TV SHOW.
// ****  ORIGIONAL PROGRAM BY MIKE MAYFIELD, MODIFIED VERSION
// ****  PUBLISHED IN DEC'S "101 BASIC GAMES", BY DAVE AHL.
// ****  MODIFICATIONS TO THE LATTER (PLUS DEBUGGING) BY BOB
// ****  LEEDOM - APRIL & DECEMBER 1974,
// ****  WITH A LITTLE HELP FROM HIS FRIENDS . . .
// ****  COMMENTS, EPITHETS, AND SUGGESTIONS SOLICITED --
// ****  SEND TO:  R. C. LEEDOM
// ****            WESTINGHOUSE DEFENSE & ELECTRONICS SYSTEMS CNTR.
// ****            BOX 746, M.S. 338
// ****            BALTIMORE, MD  21203
// ****
// ****  CONVERTED TO MICROSOFT 8 K BASIC 3/16/78 BY JOHN GORDERS
// ****  LINE NUMBERS FROM VERSION STREK7 OF 1/12/75 PRESERVED AS
// ****  MUCH AS POSSIBLE WHILE USING MULTIPLE STATEMENTS PER LINE
// ****  SOME LINES ARE LONGER THAN 72 CHARACTERS; THIS WAS DONE
// ****  BY USING "?" INSTEAD OF "PRINT" WHEN ENTERING LINES
// ****
// ****  CONVERTED TO MICROSOFT C# 2/20/21 BY ANDREW COOPER
// ****

namespace SuperStarTrek
{
    internal class Program
    {
        static void Main()
        {
            var game = new Game();

            game.DoIntroduction();

            do
            {
                game.Play();
            } while (game.Replay());
        }
    }
}
