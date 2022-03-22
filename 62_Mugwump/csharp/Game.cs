using System.Reflection;

namespace Mugwump;

internal class Game
{
    private readonly TextIO _io;
    private readonly IRandom _random;

    internal Game(TextIO io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    internal void Play(Func<bool> playAgain = null)
    {
        DisplayIntro();

        while (playAgain?.Invoke() ?? true)
        {
            Play(new Grid(_io, _random));

            _io.WriteLine();
            _io.WriteLine("That was fun! Let's play again.......");
            _io.WriteLine("Four more mugwumps are now in hiding.");
        }
    }

    private void DisplayIntro()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Mugwump.Strings.Intro.txt");

        _io.Write(stream);
    }

    private void Play(Grid grid)
    {
        for (int turn = 1; turn <= 10; turn++)
        {
            var guess = _io.ReadGuess($"Turn no. {turn} -- what is your guess");

            if (grid.Check(guess))
            {
                _io.WriteLine();
                _io.WriteLine($"You got them all in {turn} turns!");
                return;
            }
        }

        _io.WriteLine();
        _io.WriteLine("Sorry, that's 10 tries.  Here is where they're hiding:");
        grid.Reveal();
    }
}
