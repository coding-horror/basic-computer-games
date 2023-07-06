namespace Games.Common.IO;

internal static class IOExtensions
{
    internal static Position ReadPosition(this IReadWrite io) => Position.Create(io.Read2Numbers(""));

    internal static Position ReadValidPosition(this IReadWrite io)
    {
        while (true)
        {
            if (Position.TryCreateValid(io.Read2Numbers(""), out var position)) 
            { 
                return position; 
            }
            io.Write(Streams.Illegal);
        }
    }

    internal static IEnumerable<Position> ReadPositions(this IReadWrite io, string shipName, int shipSize)
    {
        io.WriteLine(shipName);
        for (var i = 0; i < shipSize; i++)
        {
             yield return io.ReadPosition();
        }
    }
}
