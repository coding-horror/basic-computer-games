namespace Queen;

internal static class RandomExtensions
{
    internal static Move NextMove(this IRandom random)
        => random.NextFloat() switch
        {
            > 0.6F => Move.Down,
            > 0.3F => Move.DownLeft,
            _ => Move.Left
        };
}
