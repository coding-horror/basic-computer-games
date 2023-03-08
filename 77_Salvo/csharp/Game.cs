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

L1040:  var humanGrid = new Grid();
        var hitTurnRecord = new int[13];
        var shots = new Position[8];
        var temp = new Position[13];
        var hitShipValue = new float[13];
L1060:  for (var i = 1; i <= 12; i++)
        {
L1070:      hitTurnRecord[i] = -1;
L1080:      hitShipValue[i] = -1;
        }
L1190:  var computerGrid = new Grid();
L1240:  for (var K = 3; K >= 0; K--)
        {
L1250:      var shipGenerationAttempts=0;
L1260:      var (start, delta) = GetRandomShipPositionInRange(K);
L1340:      shipGenerationAttempts++;
L1350:      if (shipGenerationAttempts>25) { goto L1190; }
            // determine ship position
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
                computerGrid[temp[i]] = _shipValue[K];
            }
        }
        _io.WriteLine("ENTER POSITION FOR...");
        _io.WriteLine("BATTLESHIP");
        for (var i = 1; i <= 5; i++)
        {
            humanGrid[_io.ReadPosition()] = 3;
        }
        _io.WriteLine("CRUISER");
        for (var i = 1; i <= 3; i++)
        {
            humanGrid[_io.ReadPosition()] = 2;
        }
        _io.WriteLine("DESTROYER<A>");
        for (var i = 1; i <= 2; i++)
        {
            humanGrid[_io.ReadPosition()] = 1;
        }
        _io.WriteLine("DESTROYER<B>");
        for (var i = 1; i <= 2; i++)
        {
            humanGrid[_io.ReadPosition()] = 0.5F;
        }
        var startResponse = _io.ReadString("DO YOU WANT TO START");
        while (startResponse == "WHERE ARE YOUR SHIPS?")
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
            foreach (var position in Position.All)
            {
                if (humanGrid[position] == shipValue) 
                { 
                    maxShotCount+=(int)(shipValue+.5F);
                    break;
                }
            }
        }
L2090:  for (var i = 1; i <= 7; i++)
        {
            shots[i] = temp[i] = 0;
        }
L2150:  var untriedSquareCount=0;
        foreach (var position in Position.All)
        {
            if (computerGrid[position] <= 10) { untriedSquareCount++; }
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
                var position = _io.ReadValidPosition();
L2390:          if (computerGrid[position]>10) 
                { 
                    _io.WriteLine($"YOU SHOT THERE BEFORE ON TURN {computerGrid[position]-10}");
                    continue;
                }
                shots[i]= position;
                break;
            }
        }
L2460:  for (var W = 1; W <= maxShotCount; W++)
        {
            _io.WriteLine(computerGrid[shots[W]] switch
            {
                3 => "YOU HIT MY BATTLESHIP.",
                2 => "YOU HIT MY CRUISER.",
                1 => "YOU HIT MY DESTROYER<A>.",
                .5F => "YOU HIT MY DESTROYER<B>.",
                _ => throw new InvalidOperationException($"Unexpected value {computerGrid[shots[W]]}")
            });
L2510:      computerGrid[shots[W]] = 10+turnNumber;
        }
L2620:  if (startResponse == "YES") { goto L2670; }
L2640:  turnNumber++;
L2650:  _io.WriteLine();
L2660:  _io.WriteLine($"TURN {turnNumber}");
L2670:  maxShotCount = 0;
L2680:  for (var shipValue = .5F; shipValue <= 3; shipValue += .5F)
        {
            foreach (var position in Position.All)    
            {
                if (computerGrid[position] == shipValue) 
                { 
                    maxShotCount += (int)(shipValue+.5F);
                    break;
                }
            }
        }
L2770:  untriedSquareCount=0;
        foreach (var position in Position.All)
        {
            if (computerGrid[position]<=10) { untriedSquareCount++; }
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
L3020:  var (shot, _) = GetRandomShipPosition();
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
L3310:  if (humanGrid[shot]>10) { goto L3210; }
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
L3430:      if (humanGrid[temp[i]] == 3) 
            { 
                _io.WriteLine("I HIT YOUR BATTLESHIP");
            }
            else if (humanGrid[temp[i]] == 2) 
            { 
                _io.WriteLine("I HIT YOUR CRUISER");
            }
            else if (humanGrid[temp[i]] == 1) 
            { 
                _io.WriteLine("I HIT YOUR DESTROYER<A>");
            }
            else if (humanGrid[temp[i]] == .5F) 
            { 
                _io.WriteLine("I HIT YOUR DESTROYER<B>");
            }
            else
            {
                humanGrid[temp[i]]=10+turnNumber;
                continue;
            }
L3570:      for (var j = 1; j <= 12; j++)
            {
                // record hit
L3580:          if (hitTurnRecord[j] != -1) { continue; }
L3590:          hitTurnRecord[j]=10+turnNumber;
L3600:          hitShipValue[j]=humanGrid[temp[i]];
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
L3470:      humanGrid[temp[i]]=10+turnNumber;
        }
L3490:  goto L1950;
L3800:  //REM************************USINGEARRAY
        var tempGrid = new Grid();
L3860:  for (var i = 1; i <= 12; i++)
        {
            if (hitTurnRecord[i]<10) { continue; }
            foreach (var position in Position.All)
            {
                if (humanGrid[position]>=10) 
                {  
                    foreach (var neighbour in position.Neighbours)    
                    {
                        if (humanGrid[neighbour] == hitTurnRecord[i])
                        {
                            tempGrid[position] += hitTurnRecord[i]-position.Y*(int)(hitShipValue[i]+.5F);
                        }
                    }
                }
                else
                {
                    tempGrid[position]=-10000000;
                }
            }
        }
L4030:  for (var i = 1; i <= maxShotCount; i++)
        {
L4040:      temp[i]=i;
        }
        foreach (var position in Position.All)
        {
L4090:      var Q9=1;
L4100:      for (var i = 1; i <= maxShotCount; i++)
            {
L4110:          if (tempGrid[temp[i]]>=tempGrid[temp[Q9]]) { continue; }
L4120:          Q9=i;
            }
L4131:      if (position.X>maxShotCount) { goto L4140; }
L4132:      if (position.IsOnDiagonal) { goto L4210; }
L4140:      if (tempGrid[position]<tempGrid[temp[Q9]]) { goto L4210; }
L4150:      for (var i = 1; i <= maxShotCount; i++)
            {
L4160:          if (temp[i].X != position.X) { break; }
L4170:          if (temp[i].Y == position.Y) { goto L4210; }
            }
L4190:      temp[Q9]=position;
L4210:      ;// NoOp -  NEXT S 
        }
L4230:  goto L3380;
    }

    private (Position, Offset) GetRandomShipPosition()
    {
        var startX = _random.Next(1, 11);
        var startY = _random.Next(1, 11);
        var deltaY = _random.Next(-1, 2);
        var deltaX = _random.Next(-1, 2);
        return (new(startX, startY), new(deltaX, deltaY));
    }

    private (Position, Offset) GetRandomShipPositionInRange(int shipNumber)
    {
        while (true)
        {
            var (start, delta) = GetRandomShipPosition();
            var shipSizeLessOne = _shipSize[shipNumber] - 1;
            var end = start + delta * shipSizeLessOne;
            if (delta != 0 && end.IsInRange) 
            {
                return (start, delta);
            }
        }
    }
}

internal class Grid
{
    private readonly float[,] _positions = new float[10, 10];

    public float this[Position position] 
    {
        get => _positions[position.X, position.Y];
        set => _positions[position.X, position.Y] = value;
    }
}

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
            io.WriteLine("ILLEGAL, ENTER AGAIN.");
        }
    }
}

internal record struct Position(Coordinate X, Coordinate Y)
{
    public bool IsInRange => X.IsInRange && Y.IsInRange;
    public bool IsOnDiagonal => X == Y;

    public static Position Create((float X, float Y) coordinates) => new(coordinates.X, coordinates.Y);

    public static bool TryCreateValid((float X, float Y) coordinates, out Position position)
    {
        if (Coordinate.TryCreateValid(coordinates.X, out var x) && Coordinate.TryCreateValid(coordinates.Y, out var y))
        {
            position = new(x, y);
            return true;
        }

        position = default;
        return false;
    }

    public static IEnumerable<Position> All
        => Coordinate.Range.SelectMany(x => Coordinate.Range.Select(y => new Position(x, y)));

    public IEnumerable<Position> Neighbours
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

    internal double DistanceTo(Position other)
        => Math.Sqrt((X - other.X) * (X - other.Y) + (Y - other.Y) * (Y - other.Y));

    internal Position BringIntoRange(IRandom random)
        => IsInRange ? this : new(X.BringIntoRange(random), Y.BringIntoRange(random));

    public static Position operator +(Position position, Offset offset) 
        => new(position.X + offset.X, position.Y + offset.Y);

    public static implicit operator Position(int value) => new(value, value);

    public override string ToString() => $"{X}{Y}";
}

internal record struct Coordinate(int Value)
{
    public static IEnumerable<Coordinate> Range => Enumerable.Range(0, 10).Select(v => new Coordinate(v));

    public bool IsInRange => Value is >= 0 and <= 9;

    public static Coordinate Create(float value) => new((int)value - 1);

    public static bool TryCreateValid(float value, out Coordinate coordinate)
    {
        coordinate = default;
        if (value != (int)value) { return false; }

        var result = Create(value);

        if (result.IsInRange)
        {
            coordinate = result;
            return true;
        }

        return false;
    }

    public Coordinate BringIntoRange(IRandom random)
        => Value switch
        {
            < 0 => new(0 + (int)random.NextFloat(2.5F)),
            > 9 => new(9 - (int)random.NextFloat(2.5F)),
            _ => this
        };

    public static implicit operator Coordinate(float value) => new((int)value);
    public static implicit operator int(Coordinate coordinate) => coordinate.Value;

    public static Coordinate operator +(Coordinate coordinate, int offset) => new(coordinate.Value + offset);
    public static int operator -(Coordinate a, Coordinate b) => a.Value - b.Value;

    public override string ToString() => $" {Value + 1} ";
}

internal record struct Offset(int X, int Y)
{
    public static readonly Offset Zero = 0;

    public static Offset operator *(Offset offset, int scale) => new(offset.X * scale, offset.Y * scale);

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
