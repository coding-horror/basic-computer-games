namespace Salvo.Ships;

internal sealed class Cruiser : Ship
{
    internal Cruiser(IReadWrite io) 
        : base(io) 
    { 
    }
    
    internal Cruiser(IRandom random)
        : base(random)
    {
    }

    internal override int Shots => 2;
    internal override int Size => 3;
    internal override float Value => 2;
}
