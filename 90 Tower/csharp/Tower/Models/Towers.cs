using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tower.Models
{
    internal class Towers : IEnumerable<(int, int, int)>
    {
        private readonly Needle[] _needles = new[] { new Needle(), new Needle(), new Needle() };

        public bool TryFindDisk(int disk, out int needle)
        {
            for (needle = 1; needle <= 3; needle++)
            {
                if (_needles[needle].Top == disk) { return true; }
            }

            return false;
        }

        public bool TryMoveDisk(int from, int to)
        {
            if (!_needles[from].TryGetTopDisk(out var disk))
            {
                throw new InvalidOperationException($"Needle {from} is empty");
            }

            if (_needles[to].TryPut(disk)) { return true; }

            _needles[from].TryPut(disk);
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