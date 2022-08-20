global using Games.Common.IO;
global using static LifeforTwo.Resources.Resource;

var io = new ConsoleIO();

io.Write(Streams.Title);

var _cells = new int[7, 7];
var _willLive = new[] { 3, 102, 103, 120, 130, 121, 112, 111, 12, 21, 30, 1020, 1030, 1011, 1021, 1003, 1002, 1012 };
var _offsets = new[] { -1, 0, 1, 0, 0, -1, 0, 1, -1, -1, 1, -1, -1, 1, 1, 1 };
var X = new int[3];
var Y = new int[3];
int _player1Count, _player2Count;

void CalculateNeighbors()
{
    for (var j = 1; j <= 5; j++)
    {
        for (var k = 1; k <= 5; k++)
        {
            if (_cells[j, k] > 99)
            {
                int B = _cells[j, k] > 999 ? 10 : 1;
                for (var o = 0; o < 15; o += 2)
                {
                    _cells[j + _offsets[o], k + _offsets[o + 1]] += B;
                }
            }
        }
    }
}

void CalculateAndDisplayNext()
{
    _player1Count = _player2Count = 0;
    for (var j = 0; j <= 6; j++)
    {
        io.WriteLine();
        for (var k = 0; k <= 6; k++)
        {
            if (j % 6 == 0)
            {
                io.Write($" {k % 6} ");
            }
            else if (k % 6 == 0)
            {
                io.Write($" {j % 6} ");
            }
            else
            {
                CalculateAndDisplayCell(j, k);
            }
        }
    }
    return;
}

void CalculateAndDisplayCell(int j, int k)
{
    if (_cells[j, k] >= 3)
    {
        for (var O1 = 0; O1 < 18; O1++)
        {
            if (_cells[j, k] == _willLive[O1])
            {
                if (O1 < 9)
                {
                    _cells[j, k] = 100; _player1Count++; io.Write(" * ");
                }
                else
                {
                    _cells[j, k] = 1000; _player2Count++; io.Write(" # ");
                }
                return;
            }
        }
    }

    _cells[j, k] = 0;
    io.Write("   ");
}

for (var _player = 1; _player <= 2; _player++)
{
    var P1 = _player == 2 ? 30 : 3;
    io.WriteLine(Formats.InitialPieces, _player);
    for (var i = 1; i <= 3; i++)
    {
        ReadCoordinates(_player);
        _cells[X[_player], Y[_player]] = P1;
    }
}

CalculateAndDisplayNext();

while (true)
{
    io.WriteLine();
    CalculateNeighbors();
    CalculateAndDisplayNext();

    if (_player1Count == 0 || _player2Count == 0) { break; }

    for (var _player = 1; _player <= 2; _player++)
    {
        io.WriteLine(Formats.Player, _player);
        if (ReadCoordinates(_player)) 
        { 
            _cells[X[1], Y[1]] = 100; 
            _cells[X[2], Y[2]] = 1000; 
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
        var (y, x) = io.Read2Numbers("&&&&&&\r");
        (Y[_player], X[_player]) = ((int)y, (int)x);
        if (X[_player] <= 5 && X[_player] > 0 && Y[_player] <= 5 && Y[_player] > 0 && _cells[X[_player], Y[_player]] == 0)
        {
            break;
        }
        io.Write(Streams.IllegalCoords);
    }

    if (_player == 2 && X[1] == X[2] && Y[1] == Y[2])
    {
        io.Write(Streams.SameCoords);
        // This is a bug existing in the original code. The line should be N[X[B], Y[B]] = 0;
        _cells[X[_player] + 1, Y[_player] + 1] = 0;
        return false;
    }

    return _player == 2;
}