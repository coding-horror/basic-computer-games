using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tower.Resources;

namespace Tower.Models
{
    internal class Towers : IEnumerable<(int, int, int)>
    {
        private static int[] _availableDisks = new[] { 15, 13, 11, 9, 7, 5, 3 };

        private readonly Needle[] _needles = new[] { new Needle(), new Needle(), new Needle() };
        private readonly int _smallestDisk;

        public Towers(int diskCount)
        {
            foreach (int disk in _availableDisks.Take(diskCount))
            {
                this[1].TryPut(disk);
                _smallestDisk = disk;
            }
        }

        private Needle this[int i] => _needles[i-1];

        public bool Finished => this[1].IsEmpty && this[2].IsEmpty;

        public bool TryFindDisk(int disk, out int needle, out string message)
        {
            needle = default;
            message = default;

            if (disk < _smallestDisk)
            {
                message = Strings.DiskNotInPlay;
                return false;
            }

            for (needle = 1; needle <= 3; needle++)
            {
                if (this[needle].Top == disk) { return true; }
            }

            message = Strings.DiskUnavailable;
            return false;
        }

        public bool TryMoveDisk(int from, int to)
        {
            if (!this[from].TryGetTopDisk(out var disk))
            {
                throw new InvalidOperationException($"Needle {from} is empty");
            }

            if (this[to].TryPut(disk)) { return true; }

            this[from].TryPut(disk);
            return false;
        }

        public IEnumerator<(int, int, int)> GetEnumerator() => new TowersEnumerator(_needles);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class TowersEnumerator : IEnumerator<(int, int, int)>
        {
            private readonly List<IEnumerator<int>> _enumerators;

            public TowersEnumerator(Needle[] needles)
            {
                _enumerators = needles.Select(n => n.GetEnumerator()).ToList();
            }

            public (int, int, int) Current =>
                (_enumerators[0].Current, _enumerators[1].Current, _enumerators[2].Current);

            object IEnumerator.Current => Current;

            public void Dispose() => _enumerators.ForEach(e => e.Dispose());

            public bool MoveNext() => _enumerators.All(e => e.MoveNext());

            public void Reset() => _enumerators.ForEach(e => e.Reset());
        }
    }
}