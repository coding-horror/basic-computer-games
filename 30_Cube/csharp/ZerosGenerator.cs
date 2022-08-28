namespace Cube;

internal class ZerosGenerator : IRandom
{
    public float NextFloat() => 0;

    public float PreviousFloat() => 0;

    public void Reseed(int seed) { }
}