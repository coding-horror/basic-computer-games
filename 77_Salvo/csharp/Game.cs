using Games.Common.Randomness;

namespace Salvo;

internal class Game 
{
    private static readonly float[] _shipValue = new[] { 0.5F, 1, 2, 3 };
    private static readonly int[] _shipFirstIndex = new[] { 11, 9, 6, 1 };
    private static readonly int[] _shipSize = new[] { 2, 2, 3, 5 };

    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    internal void Play()
    {
        _io.Write(Streams.Title);

L1040:  var humanGrid = new float[11,11];
        var hitTurnRecord = new int[13];
        var shots = new Coordinates[8];
        var temp = new Coordinates[13];
        var hitShipValue = new float[13];
L1060:  for (var i = 1; i <= 12; i++)
        {
L1070:      hitTurnRecord[i] = -1;
L1080:      hitShipValue[i] = -1;
        }
L1190:  var computerGrid = new float[11,11];
L1240:  for (var K = 3; K >= 0; K--)
        {
L1250:      var shipGenerationAttempts=0;
L1260:      var (start, delta) = GetRandomShipCoordinatesInRange(K);
L1340:      shipGenerationAttempts++;
L1350:      if (shipGenerationAttempts>25) { goto L1190; }
            // determine ship coordinates
L1360:      for (var i = _shipFirstIndex[K]; i <= _shipFirstIndex[K] + _shipSize[K] - 1; i++)
            {
                temp[i] = start + delta * i;
            }
L1400:      var firstIndex=_shipFirstIndex[K];
            // detect proximity to previous ships
            for (var i = firstIndex; i <= firstIndex+_shipSize[K] - 1; i++)
            {
L1415:          if (firstIndex<2) { continue; } // Only true for the Battleship
L1420:          for (var j = 1; j <= firstIndex-1; j++)
                {
L1430:              if (temp[j].DistanceTo(temp[i]) < 3.59) { goto L1260; }
                }
            }
            // put ship on board
L1460:      for (var i = firstIndex; i <= firstIndex + _shipSize[K] - 1; i++)
            {
                computerGrid[temp[i].X, temp[i].Y] = _shipValue[K];
            }
        }
L1500:  _io.WriteLine("ENTER COORDINATES FOR...");
L1510:  _io.WriteLine("BATTLESHIP");
L1520:  for (var i = 1; i <= 5; i++)
        {
            var (x, y) = _io.ReadCoordinates();
L1540:      humanGrid[x, y] = 3;
        }
L1560:  _io.WriteLine("CRUISER");
L1570:  for (var i = 1; i <= 3; i++)
        {
            var (x, y) = _io.ReadCoordinates();
L1590:      humanGrid[x, y] = 2;
        }
L1610:  _io.WriteLine("DESTROYER<A>");
L1620:  for (var i = 1; i <= 2; i++)
        {
            var (x, y) = _io.ReadCoordinates();
L1640:      humanGrid[x, y] = 1;
        }
L1660:  _io.WriteLine("DESTROYER<B>");
L1670:  for (var i = 1; i <= 2; i++)
        {
            var (x, y) = _io.ReadCoordinates();
L1690:      humanGrid[x, y] = 0.5F;
        }
L1710:  var startResponse = _io.ReadString("DO YOU WANT TO START");
L1730:  while (startResponse == "WHERE ARE YOUR SHIPS?")
        {
            _io.WriteLine("BATTLESHIP");
            for (var i = 1; i <= 5; i++)
            {
                _io.WriteLine(temp[i]);
            }
            _io.WriteLine("CRUISER");
            _io.WriteLine(temp[6]);
            _io.WriteLine(temp[7]);
            _io.WriteLine(temp[8]);
            _io.WriteLine("DESTROYER<A>");
            _io.WriteLine(temp[9]);
            _io.WriteLine(temp[10]);
            _io.WriteLine("DESTROYER<B>");
            _io.WriteLine(temp[11]);
            _io.WriteLine(temp[12]);

            startResponse = _io.ReadString("DO YOU WANT TO START");
        }
L1890:  var turnNumber=0;
L1900:  var seeShotsResponse = _io.ReadString("DO YOU WANT TO SEE MY SHOTS");
L1920:  _io.WriteLine();
L1930:  if (startResponse != "YES") { goto L2620; }
L1950:  if (startResponse != "YES") { goto L1990; }
L1960:  turnNumber++;
L1970:  _io.WriteLine();
L1980:  _io.WriteLine($"TURN {turnNumber}");
L1990:  var maxShotCount=0;
L2000:  for (var shipValue = .5F; shipValue <= 3; shipValue += .5F)
        {
L2010:      for (var x = 1; x <= 10; x++)
            {
L2020:          for (var y = 1; y <= 10; y++)
                {
L2030:              if (humanGrid[x,y] == shipValue) { goto L2070; }
                }
            }
L2060:      continue;
L2070:      maxShotCount+=(int)(shipValue+.5F);
        }
L2090:  for (var i = 1; i <= 7; i++)
        {
            shots[i] = temp[i] = 0;
        }
L2150:  var untriedSquareCount=0;
L2160:  for (var x = 1; x <= 10; x++)
        {
L2170:      for (var y = 1; y <= 10; y++)
            {
L2180:          if (computerGrid[x,y] <= 10) { untriedSquareCount++; }
            }
        }
L2220:  _io.WriteLine($"YOU HAVE {maxShotCount} SHOTS.");
        if (maxShotCount == 0) { goto L2270; }
L2230:  if (maxShotCount > untriedSquareCount) 
        { 
            _io.WriteLine("YOU HAVE MORE SHOTS THAN THERE ARE BLANK SQUARES.");
L2250:      goto L2890;
        }
L2290:  for (var i = 1; i <= maxShotCount; i++)
        {
            while (true)
            {
                var (x, y) = _io.ReadValidCoordinates();
L2390:          if (computerGrid[x,y]>10) 
                { 
                    _io.WriteLine($"YOU SHOT THERE BEFORE ON TURN {computerGrid[x,y]-10}");
                    continue;
                }
                shots[i]= new(x, y);
                break;
            }
        }
L2460:  for (var W = 1; W <= maxShotCount; W++)
        {
            _io.WriteLine(computerGrid[shots[W].X,shots[W].Y] switch
            {
                3 => "YOU HIT MY BATTLESHIP.",
                2 => "YOU HIT MY CRUISER.",
                1 => "YOU HIT MY DESTROYER<A>.",
                .5F => "YOU HIT MY DESTROYER<B>.",
                _ => throw new InvalidOperationException($"Unexpected value {computerGrid[shots[W].X,shots[W].Y]}")
            });
L2510:      computerGrid[shots[W].X,shots[W].Y] = 10+turnNumber;
        }
L2620:  if (startResponse == "YES") { goto L2670; }
L2640:  turnNumber++;
L2650:  _io.WriteLine();
L2660:  _io.WriteLine($"TURN {turnNumber}");
L2670:  maxShotCount = 0;
L2680:  for (var shipValue = .5F; shipValue <= 3; shipValue += .5F)
        {
L2690:      for (var x = 1; x <= 10; x++)
            {
L2700:          for (var y = 1; y <= 10; y++)
                {
L2710:              if (computerGrid[x,y] == shipValue) { goto L2750; }
                }
            }
L2740:      continue;
L2750:      maxShotCount += (int)(shipValue+.5F);
        }
L2770:  untriedSquareCount=0;
L2780:  for (var x = 1; x <= 10; x++)
        {
L2790:      for (var y = 1; y <= 10; y++)
            {
L2800:          if (computerGrid[x,y]>10) { continue; }
L2810:          untriedSquareCount++;
            }
        }
L2840:  _io.WriteLine($"I HAVE {maxShotCount} SHOTS.");
L2850:  if (untriedSquareCount>maxShotCount) { goto L2880; }
L2860:  _io.WriteLine("I HAVE MORE SHOTS THAN BLANK SQUARES.");
L2870:  goto L2270;
L2880:  if (maxShotCount != 0) { goto L2960; }
L2890:  _io.WriteLine("YOU HAVE WON.");
L2900:  return;
L2270:  _io.WriteLine("I HAVE WON.");
        return;


L2960:  for (var i = 1; i <= 12; i++)
        {
            // if damaged ships
L2970:      if (hitShipValue[i]>0) { goto L3800; }
        }
L3000:  var shotCount=0;
L3010:  var shotAttempts=0;
L3020:  var (shot, _) = GetRandomShipCoordinates();
L3030:  var strategyNumber=0;  //RESTORE
L3050:  shotAttempts++;
L3060:  if (shotAttempts>100) { goto L3010; }
        // ensure shot is in range
L3070:  shot = shot.BringIntoRange(_random);
L3170:  goto L3270;
        // record shot
L3180:  temp[shotCount]=shot;
L3200:  if (shotCount==maxShotCount) { goto L3380; }
L3210:  if (strategyNumber==6) { goto L3030; }
L3240:  //DATA 1,1,-1,1,1,-3,1,1,0,2,-1,1
        var data = new Offset[] { new(1,1),new(-1,1),new(1,-3),new(1,1),new(0,2),new(-1,1) };
L3220:  //READ X1,Y1
        var offset = data[strategyNumber++];
L3250:  shot+=offset;
        // is the shot in range?
L3270:  if (!shot.IsInRange) { goto L3210; }
        // have we fired here before
L3310:  if (humanGrid[shot.X,shot.Y]>10) { goto L3210; }
        // have we already selected this shot?
L3320:  for (var i = 1; i <= shotCount; i++)
        {
L3330:      if (temp[i] == shot) { goto L3210; }
        }
L3360:  shotCount++;
L3370:  goto L3180;
        // display shots
L3380:  if (seeShotsResponse != "YES") { goto L3420; }
L3390:  for (var i = 1; i <= maxShotCount; i++)
        {
L3400:      _io.WriteLine(temp[i]);
        }
L3420:  for (var i = 1; i <= maxShotCount; i++)
        {
L3430:      if (humanGrid[temp[i].X,temp[i].Y] == 3) 
            { 
                _io.WriteLine("I HIT YOUR BATTLESHIP");
            }
            else if (humanGrid[temp[i].X,temp[i].Y] == 2) 
            { 
                _io.WriteLine("I HIT YOUR CRUISER");
            }
            else if (humanGrid[temp[i].X,temp[i].Y] == 1) 
            { 
                _io.WriteLine("I HIT YOUR DESTROYER<A>");
            }
            else if (humanGrid[temp[i].X,temp[i].Y] == .5F) 
            { 
                _io.WriteLine("I HIT YOUR DESTROYER<B>");
            }
            else
            {
                humanGrid[temp[i].X,temp[i].Y]=10+turnNumber;
                continue;
            }
L3570:      for (var j = 1; j <= 12; j++)
            {
                // record hit
L3580:          if (hitTurnRecord[j] != -1) { continue; }
L3590:          hitTurnRecord[j]=10+turnNumber;
L3600:          hitShipValue[j]=humanGrid[temp[i].X,temp[i].Y];
                // look for past hits on same ship
L3610:          var shipHits=0;
L3620:          for (var k = 1; k <= 12; k++)
                {
L3630:              if (hitShipValue[k] != hitShipValue[j]) { continue; }
L3640:              shipHits++;
                }
                // if ship is not sunk
L3660:          if (shipHits != (int)(hitShipValue[j]+.5F)+1+(int)(hitShipValue[j]+.5F)/3) { goto L3470; }
                // otherwise, remove ship hit records
L3670:          for (var k = 1; k <= 12; k++)
                {
L3680:              if (hitShipValue[k] == hitShipValue[j]) 
                    {
L3700:                  hitShipValue[k] = hitTurnRecord[k] = -1;
                    }
                }
L3720:          goto L3470;
            }
            // we shouldn't get here
L3740:      _io.WriteLine("PROGRAM ABORT:");
L3750:      for (var j = 1; j <= 12; j++)
            {
L3760:          _io.WriteLine($"{nameof(hitTurnRecord)}( {j} ) = {hitTurnRecord[j]}");
L3770:          _io.WriteLine($"{nameof(hitShipValue)}( {j} ) = {hitShipValue[j]}");
            }
            return;
L3470:      humanGrid[temp[i].X,temp[i].Y]=10+turnNumber;
        }
L3490:  goto L1950;
L3800:  //REM************************USINGEARRAY
        var tempGrid = new int[11,11];
L3860:  for (var i = 1; i <= 12; i++)
        {
            if (hitTurnRecord[i]<10) { continue; }
            foreach (var position in Coordinates.All)
            {
                if (humanGrid[position.X,position.Y]>=10) 
                {  
                    foreach (var neighbour in position.Neighbours)    
                    {
                        if (humanGrid[neighbour.X,neighbour.Y] == hitTurnRecord[i])
                        {
                            tempGrid[position.X,position.Y] += hitTurnRecord[i]-position.Y*(int)(hitShipValue[i]+.5F);
                        }
                    }
                }
                else
                {
                    tempGrid[position.X,position.Y]=-10000000;
                }
            }
        }
L4030:  for (var i = 1; i <= maxShotCount; i++)
        {
L4040:      temp[i]=i;
        }
        foreach (var position in Coordinates.All)
        {
L4090:      var Q9=1;
L4100:      for (var i = 1; i <= maxShotCount; i++)
            {
L4110:          if (tempGrid[temp[i].X,temp[i].Y]>=tempGrid[temp[Q9].X,temp[Q9].Y]) { continue; }
L4120:          Q9=i;
            }
L4131:      if (x>maxShotCount) { goto L4140; }
L4132:      if (x==y) { goto L4210; }
L4140:      if (tempGrid[x,y]<tempGrid[temp[Q9].X,temp[Q9].Y]) { goto L4210; }
L4150:      for (var i = 1; i <= maxShotCount; i++)
            {
L4160:          if (temp[i].X != x) { break; }
L4170:          if (temp[i].Y == y) { goto L4210; }
            }
L4190:      temp[Q9]=position;
L4210:      x=x;// NoOp -  NEXT S 
        }
L4230:  goto L3380;

    }

    private (Coordinates, Offset) GetRandomShipCoordinates()
    {
        var startX = _random.Next(1, 11);
        var startY = _random.Next(1, 11);
        var deltaY = _random.Next(-1, 2);
        var deltaX = _random.Next(-1, 2);
        return (new(startX, startY), new(deltaX, deltaY));
    }

    private (Coordinates, Offset) GetRandomShipCoordinatesInRange(int shipNumber)
    {
        while (true)
        {
            var (start, delta) = GetRandomShipCoordinates();
            var shipSizeLessOne = _shipSize[shipNumber] - 1;
            var end = start + delta * shipSizeLessOne;
            if (delta != 0 && end.IsInRange) 
            {
                return (start, delta);
            }
        }
    }
}

internal static class IOExtensions
{
    internal static (int X, int Y) ReadCoordinates(this IReadWrite io)
    {
        var (x, y) = io.Read2Numbers("");
        return ((int)x, (int)y);
    }

    internal static (int X, int Y) ReadValidCoordinates(this IReadWrite io)
    {
        while (true)
        {
            var (x, y) = io.Read2Numbers("");
            if (x == (int)x && x is >= 1 and <= 10 && y == (int)y && y is >= 1 and <= 10) 
            { 
                return ((int)x, (int)y); 
            }
            io.WriteLine("ILLEGAL, ENTER AGAIN.");
        }
    }
}

internal record struct Coordinates(int X, int Y)
{
    internal Coordinates(float x, float y) 
        : this((int)x, (int)y) 
    { 
    }

    public bool IsInRange => X is >= 1 and <= 10 && Y is >= 1 and <= 10;

    public static IEnumerable<Coordinates> All
    {
        get
        {
            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    yield return new(x, y);
                }
            }
        }
    }

    public IEnumerable<Coordinates> Neighbours
    {
        get
        {
            foreach (var offset in Offset.Units)
            {
                var neighbour = this + offset;
                if (neighbour.IsInRange) { yield return neighbour; }
            }
        }
    }

    internal double DistanceTo(Coordinates other)
        => Math.Sqrt((X - other.X) * (X - other.Y) + (Y - other.Y) * (Y - other.Y));

    internal Coordinates BringIntoRange(IRandom random)
        => IsInRange ? this : new(BringIntoRange(X, random), BringIntoRange(Y, random));

    private int BringIntoRange(int value, IRandom random)
        => value switch
        {
            < 1 => 1 + (int)random.NextFloat(2.5F),
            > 10 => 10 - (int)random.NextFloat(2.5F),
            _ => value
        };

    public static Coordinates operator +(Coordinates coordinates, Offset offset) 
        => new(coordinates.X + offset.X, coordinates.Y + offset.Y);

    public static implicit operator Coordinates(int value) => new(value, value);

    public override string ToString() => $" {X}  {Y} ";
}

internal record struct Offset(int X, int Y)
{
    public static readonly Offset Zero = 0;

    public static Offset operator *(Offset offset, int scale) 
        => new(offset.X * scale, offset.Y * scale);

    public static implicit operator Offset(int value) => new(value, value);

    public static IEnumerable<Offset> Units
    {
        get
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var offset = new Offset(x, y);
                    if (offset != Zero) { yield return offset; }
                }
            }
        }
    }
}
