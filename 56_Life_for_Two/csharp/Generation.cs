internal class Generation
{
    private readonly Board _board;

    public Generation(Board board)
    {
        _board = board;
        CountNeighbours();
    }

    public Board Board => _board;

    public int Player1Count => _board.Player1Count;
    public int Player2Count => _board.Player2Count;

    public string? Result => 
        (Player1Count, Player2Count) switch
        {
            (0, 0) => Strings.Draw,
            (_, 0) => string.Format(Formats.Winner, 1),
            (0, _) => string.Format(Formats.Winner, 2),
            _ => null
        };

    public static Generation Create(IReadWrite io)
    {
        var board = new Board();

        SetInitialPieces(1, coord => board.AddPlayer1Piece(coord));
        SetInitialPieces(2, coord => board.AddPlayer2Piece(coord));

        return new Generation(board);

        void SetInitialPieces(int player, Action<Coordinates> setPiece)
        {
            io.WriteLine(Formats.InitialPieces, player);
            for (var i = 1; i <= 3; i++)
            {
                setPiece(io.ReadCoordinates(board));
            }
        }
    }

    public Generation CalculateNextGeneration()
    {
        var board = new Board();

        for (var x = 1; x <= 5; x++)
        {
            for (var y = 1; y <= 5; y++)
            {
                board[x, y] = _board[x, y].GetNext();
            }
        }

        return new(board);
    }
    
    public void AddPieces(IReadWrite io)
    {
        var player1Coordinate = io.ReadCoordinates(1, _board);
        var player2Coordinate = io.ReadCoordinates(2, _board);

        if (player1Coordinate == player2Coordinate)
        {
            io.Write(Streams.SameCoords);
            // This is a bug existing in the original code. The line should be _board[_coordinates[_player]] = 0;
            _board.ClearCell(player1Coordinate + 1);
        }
        else
        {
            _board.AddPlayer1Piece(player1Coordinate);
            _board.AddPlayer2Piece(player2Coordinate);
        }
    }

    private void CountNeighbours()
    {
        for (var x = 1; x <= 5; x++)
        {
            for (var y = 1; y <= 5; y++)
            {
                var coordinates = new Coordinates(x, y);
                var piece = _board[coordinates];
                if (piece.IsEmpty) { continue; }

                foreach (var neighbour in coordinates.GetNeighbors())
                {
                    _board[neighbour] = _board[neighbour].AddNeighbour(piece);
                }
            }
        }
    }

    public override string ToString() => _board.ToString();
}