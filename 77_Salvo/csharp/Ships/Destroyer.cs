namespace Salvo.Ships;

internal sealed class Destroyer : Ship
{
    internal Destroyer(string nameIndex, IReadWrite io)
        : base(io, $"<{nameIndex}>")
    {
    }

    internal Destroyer(string nameIndex, IRandom random)
        : base(random, $"<{nameIndex}>")
    {
    }

    internal override int Shots => 1;
    internal override int Size => 2;
    internal override float Value => Name.EndsWith("<A>") ? 1 : 0.5F;
}
