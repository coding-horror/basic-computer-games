namespace OneCheck;

internal class Move
{
    public int From { get; init; }
    public int To { get; init; }
    public int Jumped => (From + To) / 2;

    public bool IsInRange => From >= 0 && From <= 63 && To >= 0 && To <= 63;
    public bool IsTwoSpacesDiagonally => RowDelta == 2 && ColumnDelta == 2;
    private int RowDelta => Math.Abs(From / 8 - To / 8);
    private int ColumnDelta => Math.Abs(From % 8 - To % 8);
}