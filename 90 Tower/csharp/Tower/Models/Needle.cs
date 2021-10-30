using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tower.Models
{
    internal class Needle : IEnumerable<int>
    {
        private readonly Stack<int> _disks = new Stack<int>();

        public int Top => _disks.TryPeek(out var disc) ? disc : default;

        public bool TryPut(int disc)
        {
            if (_disks.Count == 0 || disc < _disks.Peek())
            {
                _disks.Push(disc);
                return true;
            }

            return false;
        }

        public bool TryGetTopDisk(out int disk) => _disks.TryPop(out disk);

        public IEnumerator<int> GetEnumerator() =>
            Enumerable.Repeat(1, 7 - _disks.Count).Concat(_disks).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}