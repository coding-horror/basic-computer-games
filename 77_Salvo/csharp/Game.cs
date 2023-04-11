using System.Collections.Immutable;
using Games.Common.Randomness;

namespace Salvo;

internal class Game 
{
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

        var hitTurnRecord = new int[13];
        var shots = new Position[8];
        var temp = new Position[13];
        var hitShipValue = new float[13];
        
        for (var i = 1; i <= 12; i++)
        {
            hitTurnRecord[i] = -1;
            hitShipValue[i] = -1;
        }
        var computerGrid = new Grid(_random);
        var humanGrid = new Grid(_io);
        var startResponse = _io.ReadString("DO YOU WANT TO START");
        while (startResponse == "WHERE ARE YOUR SHIPS?")
        {
            foreach (var ship in computerGrid.Ships)
            {
                _io.WriteLine(ship);
            }
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
L1990:  var maxShotCount = humanGrid.Ships.Sum(s => s.Shots);
L2090:  for (var i = 1; i <= 7; i++)
        {
            shots[i] = temp[i] = 0;
        }
L2220:  _io.WriteLine($"YOU HAVE {maxShotCount} SHOTS.");
        if (maxShotCount == 0) { goto L2270; }
L2230:  if (maxShotCount > computerGrid.UntriedSquareCount) 
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
            var hit = computerGrid[shots[W]] switch
            {
                3 => "YOU HIT MY BATTLESHIP.",
                2 => "YOU HIT MY CRUISER.",
                1 => "YOU HIT MY DESTROYER<A>.",
                .5F => "YOU HIT MY DESTROYER<B>.",
                _ => null
            };
            if (hit is not null) { _io.WriteLine(); }
L2510:      computerGrid[shots[W]] = 10+turnNumber;
        }
L2620:  if (startResponse == "YES") { goto L2670; }
L2640:  turnNumber++;
L2650:  _io.WriteLine();
L2660:  _io.WriteLine($"TURN {turnNumber}");
L2670:  maxShotCount = computerGrid.Ships.Sum(s => s.Shots);
L2840:  _io.WriteLine($"I HAVE {maxShotCount} SHOTS.");
L2850:  if (humanGrid.UntriedSquareCount > maxShotCount) { goto L2880; }
L2860:  _io.WriteLine("I HAVE MORE SHOTS THAN BLANK SQUARES.");
L2270:  _io.WriteLine("I HAVE WON.");
        return;
L2880:  if (maxShotCount != 0) { goto L2960; }
L2890:  _io.WriteLine("YOU HAVE WON.");
L2900:  return;


L2960:  for (var i = 1; i <= 12; i++)
        {
            // if damaged ships
L2970:      if (hitShipValue[i]>0) { goto L3800; }
        }
L3000:  var shotCount=0;
L3010:  var shotAttempts=0;
L3020:  var (shot, _) = _random.NextShipPosition();
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
}

internal abstract class Ship
{
    private readonly List<Position> _positions = new();

    protected Ship(IReadWrite io, string? nameSuffix = null)
    {
        Name = GetType().Name + nameSuffix;
        _positions = io.ReadPositions(Name, Size).ToList();
    }

    protected Ship(IRandom random, string? nameSuffix = null)
    {
        Name = GetType().Name + nameSuffix;

        var (start, delta) = random.GetRandomShipPositionInRange(Size);
        for (var i = 0; i < Size; i++)
        {
            _positions.Add(start + delta * i);
        }
    }

    internal string Name { get; }
    internal abstract int Shots { get; }
    internal abstract int Size { get; }
    internal abstract float Value { get; }
    internal IEnumerable<Position> Positions => _positions;
    internal bool IsAfloat => _positions.Count > 0;

    internal bool IsHit(Position position) => _positions.Remove(position);

    internal float DistanceTo(Ship other)
        => _positions.SelectMany(a => other._positions.Select(b => a.DistanceTo(b))).Min();

    public override string ToString() 
        => string.Join(Environment.NewLine, _positions.Select(p => p.ToString()).Prepend(Name));
}

internal sealed class Battleship : Ship
{
    internal Battleship(IReadWrite io) 
        : base(io) 
    { 
    }

    internal Battleship(IRandom random)
        : base(random)
    {
    }

    internal override int Shots => 3;
    internal override int Size => 5;
    internal override float Value => 3;
}

internal sealed class Cruiser : Ship
{
    internal Cruiser(IReadWrite io) 
        : base(io) 
    { 
    }
    
    internal Cruiser(IRandom random)
        : base(random)
    {
    }

    internal override int Shots => 2;
    internal override int Size => 3;
    internal override float Value => 2;
}

internal sealed class Destroyer : Ship
{
    internal Destroyer(string nameIndex, IReadWrite io)
        : base(io, $"<{nameIndex}>")
    {
    }

    internal Destroyer(string nameIndex, IRandom random)
        : base(random, $"<{nameIndex}>")
    {
    }

    internal override int Shots => 1;
    internal override int Size => 2;
    internal override float Value => Name.EndsWith("<A>") ? 1 : 0.5F;
}

internal static class RandomExtensions
{
    internal static (Position, Offset) NextShipPosition(this IRandom random)
    {
        var startX = random.NextCoordinate();
        var startY = random.NextCoordinate();
        var deltaY = random.NextOffset();
        var deltaX = random.NextOffset();
        return (new(startX, startY), new(deltaX, deltaY));
    }

    private static Coordinate NextCoordinate(this IRandom random)
        => random.Next(Coordinate.MinValue, Coordinate.MaxValue + 1);

    private static int NextOffset(this IRandom random) => random.Next(-1, 2);

    internal static (Position, Offset) GetRandomShipPositionInRange(this IRandom random, int shipSize)
    {
        while (true)
        {
            var (start, delta) = random.NextShipPosition();
            var shipSizeLessOne = shipSize - 1;
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
    private readonly List<Ship> _ships;
    private readonly Dictionary<Position, int> _shots = new();

    internal Grid()
    {
        _ships = new();
    }

    internal Grid(IReadWrite io)
    {
        io.WriteLine("ENTER POSITION FOR...");
        _ships = new()
        {
            new Battleship(io),
            new Cruiser(io),
            new Destroyer("A", io),
            new Destroyer("B", io)
        };
    }

    internal Grid(IRandom random)
    {
        _ships = new();
        while (true)
        {
            _ships.Add(new Battleship(random));
            if (TryPositionShip(() => new Cruiser(random)) &&
                TryPositionShip(() => new Destroyer("A", random)) &&
                TryPositionShip(() => new Destroyer("B", random)))
            {
                break;
            } 
            _ships.Clear();
        }

        foreach (var ship in _ships)
        {
            foreach (var position in ship.Positions)
            {
                this[position] = ship.Value;
            }
        }

        bool TryPositionShip(Func<Ship> shipFactory)
        {
            while (true)
            {
                var shipGenerationAttempts = 0;
                var ship = shipFactory.Invoke();
                shipGenerationAttempts++;
                if (shipGenerationAttempts > 25) { return false; }
                foreach (var previousShip in _ships)
                {
                    if (ship.DistanceTo(previousShip) >= 3.59) { return true; }
                }
            }
        }
    }

    public float this[Position position] 
    {
        get => _shots.TryGetValue(position, out var value) 
                ? value + 10
                : _ships.FirstOrDefault(s => s.Positions.Contains(position))?.Value ?? 0;
        set
        {
            _ = _ships.FirstOrDefault(s => s.IsHit(position));
            _shots[position] = (int)value - 10;
        }
    }

    internal int UntriedSquareCount => 100 - _shots.Count;
    internal IEnumerable<Ship> Ships => _ships.AsEnumerable();
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

    internal static IEnumerable<Position> ReadPositions(this IReadWrite io, string shipName, int shipSize)
    {
        io.WriteLine(shipName);
        for (var i = 0; i < shipSize; i++)
        {
             yield return io.ReadPosition();
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

    internal float DistanceTo(Position other)
        => (float)Math.Sqrt((X - other.X) * (X - other.Y) + (Y - other.Y) * (Y - other.Y));

    internal Position BringIntoRange(IRandom random)
        => IsInRange ? this : new(X.BringIntoRange(random), Y.BringIntoRange(random));

    public static Position operator +(Position position, Offset offset) 
        => new(position.X + offset.X, position.Y + offset.Y);

    public static implicit operator Position(int value) => new(value, value);

    public override string ToString() => $"{X}{Y}";
}

internal record struct Coordinate(int Value)
{
    public const int MinValue = 0;
    public const int MaxValue = 9;

    public static IEnumerable<Coordinate> Range => Enumerable.Range(0, 10).Select(v => new Coordinate(v));

    public bool IsInRange => Value is >= MinValue and <= MaxValue;

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
            < MinValue => new(MinValue + (int)random.NextFloat(2.5F)),
            > MaxValue => new(MaxValue - (int)random.NextFloat(2.5F)),
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
