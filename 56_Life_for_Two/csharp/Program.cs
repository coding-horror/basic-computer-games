global using Games.Common.IO;
global using static LifeforTwo.Resources.Resource;
global using LifeforTwo;

var io = new ConsoleIO();

io.Write(Streams.Title);

var _board = new Board();
var _willLive = new[] { 0x0003, 0x0102, 0x0103, 0x0120, 0x0130, 0x0121, 0x0112, 0x0111, 0x0012, 
                        0x0021, 0x0030, 0x1020, 0x1030, 0x1011, 0x1021, 0x1003, 0x1002, 0x1012 };
var _coordinates = new Coordinates[3];
int _player1Count, _player2Count;

void CalculateAndDisplayNext()
{
    _player1Count = _player2Count = 0;
    for (var y = 0; y <= 6; y++)
    {
        io.WriteLine();
        for (var x = 0; x <= 6; x++)
        {
            if (y % 6 == 0)
            {
                io.Write($" {x % 6} ");
            }
            else if (x % 6 == 0)
            {
                io.Write($" {y % 6} ");
            }
            else
            {
                CalculateAndDisplayCell(x, y);
            }
        }
    }
    return;
}

void CalculateAndDisplayCell(int x, int y)
{
    if (_board[x, y] >= 3)
    {
        for (var o = 0; o < 18; o++)
        {
            if (_board[x, y] == _willLive[o])
            {
                if (o < 9)
                {
                    _board[x, y] = 0x0100; _player1Count++; io.Write(" * ");
                }
                else
                {
                    _board[x, y] = 0x1000; _player2Count++; io.Write(" # ");
                }
                return;
            }
        }
    }

    _board[x, y] = 0;
    io.Write("   ");
}

for (var _player = 1; _player <= 2; _player++)
{
    var P1 = _player == 2 ? 0x30 : 0x03;
    io.WriteLine(Formats.InitialPieces, _player);
    for (var i = 1; i <= 3; i++)
    {
        ReadCoordinates(_player);
        _board[_coordinates[_player]] = P1;
    }
}

CalculateAndDisplayNext();

while (true)
{
    io.WriteLine();
    _board.CalculateNeighbours();
    CalculateAndDisplayNext();

    if (_player1Count == 0 || _player2Count == 0) { break; }

    for (var _player = 1; _player <= 2; _player++)
    {
        io.WriteLine(Formats.Player, _player);
        if (ReadCoordinates(_player)) 
        { 
            _board[_coordinates[1]] = 0x0100; 
            _board[_coordinates[2]] = 0x1000; 
        }
    }
}

if (_player1Count == 0 && _player2Count == 0)
{
    io.Write(Streams.Draw);
}
else
{
    io.WriteLine(Formats.Winner, _player2Count == 0 ? 1 : 2);
}

bool ReadCoordinates(int _player)
{
    while (true)
    {
        io.WriteLine("X,Y");
        var values = io.Read2Numbers("&&&&&&\r");
        if (Coordinates.TryCreate(values, out _coordinates[_player]) && _board[_coordinates[_player]] == 0)
        {
            break;
        }
        io.Write(Streams.IllegalCoords);
    }

    if (_player == 2 && _coordinates[1] == _coordinates[2])
    {
        io.Write(Streams.SameCoords);
        // This is a bug existing in the original code. The line should be _board[_coordinates[_player]] = 0;
        _board[_coordinates[_player] + 1] = 0;
        return false;
    }

    return _player == 2;
}
