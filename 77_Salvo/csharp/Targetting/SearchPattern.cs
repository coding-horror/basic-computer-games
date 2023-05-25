using System.Collections.Immutable;

namespace Salvo.Targetting;

internal class SearchPattern
{
    private static readonly ImmutableArray<Offset> _offsets =
        ImmutableArray.Create<Offset>(new(1, 1), new(-1, 1), new(1, -3), new(1, 1), new(0, 2), new(-1, 1));

    private int _nextIndex;

    internal bool TryGetOffset(out Offset offset)
    {
        offset = default;
        if (_nextIndex >= _offsets.Length) { return false; }
        
        offset = _offsets[_nextIndex++];
        return true;
    }

    internal void Reset() => _nextIndex = 0;
}