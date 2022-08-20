global using Games.Common.IO;
global using static LifeforTwo.Resources.Resource;

var io = new ConsoleIO();

io.Write(Streams.Title);

var N = new int[7, 7];
var K = new[] { 3, 102, 103, 120, 130, 121, 112, 111, 12, 21, 30, 1020, 1030, 1011, 1021, 1003, 1002, 1012 };
var A = new[] { -1, 0, 1, 0, 0, -1, 0, 1, -1, -1, 1, -1, -1, 1, 1, 1 };
var X = new int[3];
var Y = new int[3];
int M2, M3;

void L50()
{
    for (var j = 1; j <= 5; j++)
    {
        for (var k = 1; k <= 5; k++)
        {
            if (N[j, k] > 99)
            {
                L200(j, k);
            }
        }
    }
    L90();
}

void L90()
{
    M2 = M3 = 0;
    for (var j = 0; j <= 6; j++)
    {
        io.WriteLine();
        for (var k = 0; k <= 6; k++)
        {
            if (j == 0 || j == 6)
            {
                if (k == 6) { io.Write(" 0 "); break; }
                io.Write($" {k} ");
            }
            else if (k == 0 || k == 6)
            {
                if (j == 6) { io.WriteLine(" 0 "); return; }
                io.Write($" {j} ");
            }
            else
            {
                L300(j, k);
            }
        }
    }
    return;

}
void L200(int j, int k)
{
    int B = N[j, k] > 999 ? 10 : 1;
    for (var O1 = 0; O1 < 15; O1 += 2)
    {
        N[j + A[O1], k + A[O1 + 1]] = N[j + A[O1], k + A[O1 + 1]] + B;
    }
}
void L300(int j, int k)
{
    if (N[j, k] >= 3)
    {
        for (var O1 = 0; O1 < 18; O1++)
        {
            if (N[j, k] == K[O1])
            {
                if (O1 < 9)
                {
                    N[j, k] = 100; M2++; io.Write(" * ");
                    return;
                }
                else
                {
                    N[j, k] = 1000; M3++; io.Write(" # ");
                    return;
                }
            }
        }
    }

    N[j, k] = 0; io.Write("   ");
}

for (var j = 1; j <= 5; j++)
{
    for (var k = 1; k <= 5; k++)
    {
        N[j, k] = 0;
    }
}
for (var B = 1; B <= 2; B++)
{
    var P1 = B == 2 ? 30 : 3;
    io.WriteLine(); io.WriteLine($"PLAYER {B}  - 3 LIVE PIECES.");
    for (var K1 = 1; K1 <= 3; K1++)
    {
        L700(B);
        N[X[B], Y[B]] = P1;
    }
}
L90();
while (true)
{
    io.WriteLine();
    L50();

    if (M2 == 0 && M3 == 0) { io.WriteLine(); io.WriteLine("A DRAW"); return; }
    else if (M3 == 0) { var B = 1; io.WriteLine(); io.WriteLine($"PLAYER {B} IS THE WINNER"); return; }
    else if (M2 == 0) { var B = 2; io.WriteLine($"PLAYER {B} IS THE WINNER"); return; }

    for (var B = 1; B <= 2; B++)
    {
        io.WriteLine();
        io.WriteLine();
        io.WriteLine($"PLAYER {B}");
        B = L700(B);
        if (B == 2) { N[X[1], Y[1]] = 100; N[X[2], Y[2]] = 1000; }
    }
}

int L700(int B)
{
    while (true)
    {
        io.WriteLine("X,Y");
        var (y, x) = io.Read2Numbers("&&&&&&\r");
        (Y[B], X[B]) = ((int)y, (int)x);
        if (X[B] <= 5 && X[B] > 0 && Y[B] <= 5 && Y[B] > 0 && N[X[B], Y[B]] == 0)
        {
            break;
        }
        io.WriteLine("Illegal Coords. Retype");
    }

    if (B == 2 && X[1] == X[2] && Y[1] == Y[2])
    {
        io.WriteLine("SAME COORD.  SET TO 0");
        N[X[B] + 1, Y[B] + 1] = 0;
        B = 99;
    }

    return B;
}