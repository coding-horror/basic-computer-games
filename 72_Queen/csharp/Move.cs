namespace Queen;

internal record struct Move(int Diagonal, int Row)
{
    public static readonly Move Left = new(1, 0);
    public static readonly Move DownLeft = new(2, 1);
    public static readonly Move Down = new(1, 1);

    public bool IsValid => Diagonal > 0 && (IsLeft || IsDown || IsDownLeft);
    private bool IsLeft => Row == 0;
    private bool IsDown => Row == Diagonal;
    private bool IsDownLeft => Row * 2 == Diagonal;

    public static Move operator *(Move move, int scale) => new(move.Diagonal * scale, move.Row * scale);
}