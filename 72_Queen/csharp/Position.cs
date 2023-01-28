namespace Queen;

internal record struct Position(int Diagonal, int Row)
{
    public static readonly Position Zero = new(0);

    public Position(int number)
        : this(Diagonal: number / 10, Row: number % 10)
    {
    }

    public bool IsZero => Row == 0 && Diagonal == 0;
    public bool IsStart => Row == 1 || Row == Diagonal;
    public bool IsEnd => Row == 8 && Diagonal == 15;

    public override string ToString() => $"{Diagonal}{Row}";

    public static implicit operator Position(int value) => new(value);

    public static Position operator +(Position position, Move move)
        => new(Diagonal: position.Diagonal + move.Diagonal, Row: position.Row + move.Row);
    public static Move operator -(Position to, Position from)
        => new(Diagonal: to.Diagonal - from.Diagonal, Row: to.Row - from.Row);
}
