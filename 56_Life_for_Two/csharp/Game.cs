internal class Game
{
    private readonly IReadWrite _io;

    public Game(IReadWrite io)
    {
        _io = io;
    }

    public void Play()
    {
        _io.Write(Streams.Title);

        var generation = Generation.Create(_io);

        _io.Write(generation);

        while(true)
        {
            generation = generation.CalculateNextGeneration();
            _io.WriteLine();
            _io.Write(generation);

            if (generation.Result is not null) { break; }

            var player1Coordinate = _io.ReadCoordinates(1, generation.Board);
            var player2Coordinate = _io.ReadCoordinates(2, generation.Board);

            if (player1Coordinate == player2Coordinate)
            {
                _io.Write(Streams.SameCoords);
                // This is a bug existing in the original code. The line should be _board[_coordinates[_player]] = 0;
                generation.Board.ClearCell(player1Coordinate + 1);
            }
            else
            {
                generation.Board.AddPlayer1Piece(player1Coordinate);
                generation.Board.AddPlayer2Piece(player2Coordinate);
            }
        }

        _io.WriteLine(generation.Result);
    }
}
