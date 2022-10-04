using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace LifeforTwo;

public struct Piece
{
    public const int None = 0x0000;
    public const int Player1 = 0x0100;
    public const int Player2 = 0x1000;
    private const int PieceMask = Player1 | Player2;
    private const int NeighbourValueOffset = 8;

    private static readonly ImmutableHashSet<int> _willBePlayer1 = 
        new[] { 0x0003, 0x0102, 0x0103, 0x0120, 0x0130, 0x0121, 0x0112, 0x0111, 0x0012 }.ToImmutableHashSet();
    private static readonly ImmutableHashSet<int> _willBePlayer2 = 
        new[] { 0x0021, 0x0030, 0x1020, 0x1030, 0x1011, 0x1021, 0x1003, 0x1002, 0x1012 }.ToImmutableHashSet();

    private int _value;

    private Piece(int value) => _value = value;

    public int Value => _value & PieceMask;
    public bool IsEmpty => (_value & PieceMask) == None;

    public static Piece NewNone() => new(None);
    public static Piece NewPlayer1() => new(Player1);
    public static Piece NewPlayer2() => new(Player2);

    public Piece AddNeighbour(Piece neighbour)
    {
        _value += neighbour.Value >> NeighbourValueOffset;
        return this;
    }

    public Piece GetNext() => new(
        _value switch
        {
            _ when _willBePlayer1.Contains(_value) => Player1,
            _ when _willBePlayer2.Contains(_value) => Player2,
            _ => None
        });

    public override string ToString() =>
        (_value & PieceMask) switch
        {
            Player1 => "*",
            Player2 => "#",
            _ => " "
        };

    public static implicit operator Piece(int value) => new(value);
    public static implicit operator int(Piece piece) => piece.Value;
}