namespace Salvo.Ships;

internal sealed class Battleship : Ship
{
    internal Battleship(IReadWrite io) 
        : base(io) 
    { 
    }

    internal Battleship(IRandom random)
        : base(random)
    {
    }

    internal override int Shots => 3;
    internal override int Size => 5;
}
