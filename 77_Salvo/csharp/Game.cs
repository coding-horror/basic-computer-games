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

        var hitTurnRecord = new int[13];
        var temp = new Position[13];
        var hitShipValue = new float[13];
        
        for (var i = 1; i <= 12; i++)
        {
            hitTurnRecord[i] = -1;
            hitShipValue[i] = -1;
        }
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
            if (computerGrid.IsHit(shot1, turnNumber, out var shipName))
            {
                _io.Write(Strings.YouHit(shipName));
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

L2960:  for (var i = 1; i <= 12; i++)
        {
            // if damaged ships
L2970:      if (hitShipValue[i]>0) { goto L3800; }
        }
        temp = computerShotSelector.GetShots().ToArray();
        // display shots
L3380:  if (seeShotsResponse == "yes") 
        {
            foreach (var shot in temp)
            {
                _io.WriteLine(shot);
            }
        }
        foreach (var shot in temp)
        {
            if (!humanGrid.IsHit(shot, turnNumber, out var shipName))
            { 
                continue;
            }
            _io.WriteLine(Strings.IHit(shipName));
L3570:      for (var j = 1; j <= 12; j++)
            {
                // record hit
L3580:          if (hitTurnRecord[j] != -1) { continue; }
L3590:          hitTurnRecord[j]=10+turnNumber;
L3600:          hitShipValue[j]=humanGrid[shot];
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
L3470:      humanGrid[shot]=10+turnNumber;
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
L4030:  for (var i = 1; i <= numberOfShots; i++)
        {
L4040:      temp[i]=i;
        }
        foreach (var position in Position.All)
        {
L4090:      var Q9=1;
L4100:      for (var i = 1; i <= numberOfShots; i++)
            {
L4110:          if (tempGrid[temp[i]]>=tempGrid[temp[Q9]]) { continue; }
L4120:          Q9=i;
            }
L4131:      if (position.X>numberOfShots) { goto L4140; }
L4132:      if (position.IsOnDiagonal) { goto L4210; }
L4140:      if (tempGrid[position]<tempGrid[temp[Q9]]) { goto L4210; }
L4150:      for (var i = 1; i <= numberOfShots; i++)
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
