using System;
using System.Linq;
using Games.Common.IO;

namespace Hexapawn;

// Provides input methods which emulate the BASIC interpreter's keyboard input routines
internal static class IReadWriteExtensions
{
    internal static char GetYesNo(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var response = io.ReadString($"{prompt} (Y-N)").FirstOrDefault();
            if ("YyNn".Contains(response))
            {
                return char.ToUpperInvariant(response);
            }
        }
    }

    // Implements original code:
    //   120 PRINT "YOUR MOVE";
    //   121 INPUT M1,M2
    //   122 IF M1=INT(M1)AND M2=INT(M2)AND M1>0 AND M1<10 AND M2>0 AND M2<10 THEN 130
    //   123 PRINT "ILLEGAL CO-ORDINATES."
    //   124 GOTO 120
    internal static Move ReadMove(this IReadWrite io, string prompt)
    {
        while(true)
        {
            var (from, to) = io.Read2Numbers(prompt);

            if (Move.TryCreate(from, to, out var move))
            {
                return move;
            }

            io.WriteLine("Illegal Coordinates.");
        }
    }
}
