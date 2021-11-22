using System;
using System.Collections.Generic;
using System.Linq;

namespace Mugwump
{
    internal class Grid
    {
        private readonly List<Mugwump> _mugwumps;

        public Grid(IEnumerable<Mugwump> mugwumps)
        {
            _mugwumps = mugwumps.ToList();
        }

        public bool Check(Position guess)
        {
            foreach (var mugwump in _mugwumps.ToList())
            {
                var (found, distance) = mugwump.FindFrom(guess);

                Console.WriteLine(found ? $"You have found {mugwump}" : $"You are {distance} units from {mugwump}");
                if (found)
                {
                    _mugwumps.Remove(mugwump);
                }
            }

            return _mugwumps.Count == 0;
        }

        public void Reveal()
        {
            foreach (var mugwump in _mugwumps.ToList())
            {
                Console.WriteLine(mugwump.Reveal());
            }
        }
    }
}
