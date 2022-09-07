global using Games.Common.IO;
global using static LifeforTwo.Resources.Resource;
global using LifeforTwo;

var io = new ConsoleIO();

io.Write(Streams.Title);

var _board = new Board();

for (var _player = 1; _player <= 2; _player++)
{
    var P1 = _player == 2 ? 0x30 : 0x03;
    io.WriteLine(Formats.InitialPieces, _player);
    for (var i = 1; i <= 3; i++)
    {
        _board[io.ReadCoordinates(_board)] = P1;
    }
}

_board.CalculateNextGeneration();
_board.Display(io);

while (true)
{
    io.WriteLine();
    _board.CalculateNeighbours();
    _board.CalculateNextGeneration();
    _board.Display(io);

    if (_board.Result is not null) { break; }

    var player1Coordinate = io.ReadCoordinates(1, _board);
    var player2Coordinate = io.ReadCoordinates(2, _board);

    if (player1Coordinate == player2Coordinate)
    {
        io.Write(Streams.SameCoords);
        // This is a bug existing in the original code. The line should be _board[_coordinates[_player]] = 0;
        _board[player1Coordinate + 1] = 0;
    }
    else
    {
        _board[player1Coordinate] = 0x0100;
        _board[player2Coordinate] = 0x1000;
    }
}

io.WriteLine(_board.Result);