using System.Collections.Generic;
using System.Linq;

namespace Mugwump;

internal class Grid
{
    private readonly TextIO _io;
    private readonly List<Mugwump> _mugwumps;

    public Grid(TextIO io, IRandom random)
    {
        _io = io;
        _mugwumps = Enumerable.Range(1, 4).Select(id => new Mugwump(id, random.NextPosition(10, 10))).ToList();
    }

    public bool Check(Position guess)
    {
        foreach (var mugwump in _mugwumps.ToList())
        {
            var (found, distance) = mugwump.FindFrom(guess);

            _io.WriteLine(found ? $"You have found {mugwump}" : $"You are {distance} units from {mugwump}");
            if (found)
            {
                _mugwumps.Remove(mugwump);
            }
        }

        return _mugwumps.Count == 0;
    }

    public void Reveal()
    {
        foreach (var mugwump in _mugwumps)
        {
            _io.WriteLine(mugwump.Reveal());
        }
    }
}
