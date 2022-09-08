internal static class IOExtensions
{
    internal static Coordinates ReadCoordinates(this IReadWrite io, int player, Board board)
    {
        io.Write(Formats.Player, player);
        return io.ReadCoordinates(board);
    }

    internal static Coordinates ReadCoordinates(this IReadWrite io, Board board)
    {
        while (true)
        {
            io.WriteLine("X,Y");
            var values = io.Read2Numbers("&&&&&&\r");
            if (Coordinates.TryCreate(values, out var coordinates) && board[coordinates] == 0)
            {
                return coordinates;
            }
            io.Write(Streams.IllegalCoords);
        }
    }
}