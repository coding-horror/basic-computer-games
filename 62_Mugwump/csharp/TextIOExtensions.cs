namespace Mugwump;

// Provides input methods which emulate the BASIC interpreter's keyboard input routines
internal static class TextIOExtensions
{
    internal static Position ReadGuess(this TextIO io, string prompt)
    {
        io.WriteLine();
        io.WriteLine();
        var (x, y) = io.Read2Numbers(prompt);
        return new Position(x, y);
    }
}
