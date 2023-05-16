using Salvo.Targetting;

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

        var hitRecords = new List<(int Turn, Ship Ship)>();
        var temp = new Position[13];
        
        var computerGrid = new Grid(_random);
        var humanGrid = new Grid(_io);
        var humanShotSelector = new HumanShotSelector(humanGrid, computerGrid, _io);
        var computerShotSelector = new SearchPatternShotSelector(computerGrid, humanGrid, _random);
        var startResponse = _io.ReadString(Prompts.Start);
        while (startResponse == Strings.WhereAreYourShips)
        {
            foreach (var ship in computerGrid.Ships)
            {
                _io.WriteLine(ship);
            }
            startResponse = _io.ReadString(Prompts.Start);
        }
L1890:  var turnNumber=0;
L1900:  var seeShotsResponse = _io.ReadString(Prompts.SeeShots);
L1920:  _io.WriteLine();
L1930:  if (startResponse != "YES") { goto L2620; }
L1950:  if (startResponse != "YES") { goto L1990; }
L1960:  turnNumber++;
L1980:  _io.Write(Strings.Turn(turnNumber));
L1990:  var numberOfShots = humanShotSelector.NumberOfShots;
L2220:  _io.Write(Strings.YouHaveShots(numberOfShots));
        if (numberOfShots == 0) { goto L2270; }
L2230:  if (numberOfShots > computerGrid.UntriedSquareCount) 
        { 
            _io.WriteLine(Streams.YouHaveMoreShotsThanSquares);
L2250:      goto L2890;
        }
        foreach (var shot1 in humanShotSelector.GetShots())
        {
            if (computerGrid.IsHit(shot1, turnNumber, out var ship))
            {
                _io.Write(Strings.YouHit(ship.Name));
            }
        }
L2620:  if (startResponse == "YES") { goto L2670; }
L2640:  turnNumber++;
L2660:  _io.Write(Strings.Turn(turnNumber));
L2670:  numberOfShots = computerShotSelector.NumberOfShots;
L2840:  _io.Write(Strings.IHaveShots(numberOfShots));
L2850:  if (humanGrid.UntriedSquareCount > numberOfShots) { goto L2880; }
L2860:  _io.Write(Streams.IHaveMoreShotsThanSquares);
L2270:  _io.Write(Streams.IWon);
        return;
L2880:  if (numberOfShots != 0) { goto L2960; }
L2890:  _io.Write(Streams.YouWon);
L2900:  return;

L2960:  if (humanGrid.Ships.Any(s => s.IsDamaged)) { goto L3800; }
        temp = computerShotSelector.GetShots().ToArray();
        // display shots
L3380:  if (seeShotsResponse == "YES") 
        {
            foreach (var shot in temp)
            {
                _io.WriteLine(shot);
            }
        }
        foreach (var shot in temp)
        {
            if (!humanGrid.IsHit(shot, turnNumber, out var ship))
            { 
                continue;
            }
            _io.Write(Strings.IHit(ship.Name));
            if (ship.IsDestroyed) 
            {
                hitRecords = hitRecords.Where(hr => hr.Ship != ship).ToList();
            }
            else
            {
                hitRecords.Add((turnNumber, ship));
            }
        }
        goto L1950;
L3800:  //REM************************USINGEARRAY
        var tempGrid = Position.All.ToDictionary(x => x, _ => 0);
L3860:  foreach (var (hitTurn, ship) in hitRecords)
        {
            foreach (var position in Position.All)
            {
                if (humanGrid.WasTargetedAt(position, out _))
                {  
                    tempGrid[position]=-10000000;
                    continue;
                }

                foreach (var neighbour in position.Neighbours)    
                {
                    if (humanGrid.WasTargetedAt(neighbour, out var turn) && turn == hitTurn)
                    {
                        tempGrid[position] += hitTurn + 10 - position.Y * ship.Shots;
                    }
                }
            }
        }
L4030:  for (var i = 0; i < numberOfShots; i++)
        {
L4040:      temp[i]=i+1;
        }
        foreach (var position in Position.All)
        {
L4090:      var Q9=0;
L4100:      for (var i = 0; i < numberOfShots; i++)
            {
L4110:          if (tempGrid[temp[i]] < tempGrid[temp[Q9]]) 
                { 
L4120:              Q9 = i;
                }
            }
L4131:      if (position.X <= numberOfShots && position.IsOnDiagonal) { continue; }
L4140:      if (tempGrid[position]<tempGrid[temp[Q9]]) { continue; }
            if (!temp.Contains(position))
            {
                temp[Q9] = position;
            }
        }
L4230:  goto L3380;
    }
}

internal class DataRandom : IRandom
{
    private readonly Queue<float> _values = 
        new(File.ReadAllLines("data.txt").Select(l => float.Parse(l) / 1000000));
    private float _previous;

    public float NextFloat() => _previous = _values.Dequeue();

    public float PreviousFloat() => _previous;

    public void Reseed(int seed)
    {
        throw new NotImplementedException();
    }
}

