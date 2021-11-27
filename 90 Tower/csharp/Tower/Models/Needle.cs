using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tower.Models
{
    internal class Needle : IEnumerable<int>
    {
        private readonly Stack<int> _disks = new Stack<int>();

        public bool IsEmpty => _disks.Count == 0;

        public int Top => _disks.TryPeek(out var disk) ? disk : default;

        public bool TryPut(int disk)
        {
            if (_disks.Count == 0 || disk < _disks.Peek())
            {
                _disks.Push(disk);
                return true;
            }

            return false;
        }

        public bool TryGetTopDisk(out int disk) => _disks.TryPop(out disk);

        public IEnumerator<int> GetEnumerator() =>
            Enumerable.Repeat(0, 7 - _disks.Count).Concat(_disks).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}