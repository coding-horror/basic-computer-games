namespace OneCheck;

internal class Board
{
    private readonly int[][] _checkers;

    public Board()
    {
        _checkers = 
            Enumerable.Range(0, 8)
                .Select(r => Enumerable.Range(0, 8)
                    .Select(c => r > 1 && r < 6 && c > 1 && c < 6 ? 0 : 1).ToArray())
                .ToArray();
    }

    public override string ToString() => 
        string.Join(Environment.NewLine, _checkers.Select(r => string.Join(" ", r.Select(c => $" {c}"))));
}