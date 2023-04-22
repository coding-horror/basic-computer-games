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
        var humanShotSelector = new HumanShotSelector(humanGrid, computerGrid);
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
L1990:  var numberOfShots = humanShotSelector.GetShotCount();
L2220:  _io.Write(Strings.YouHaveShots(numberOfShots));
        if (numberOfShots == 0) { goto L2270; }
L2230:  if (numberOfShots > computerGrid.UntriedSquareCount) 
        { 
            _io.WriteLine(Streams.YouHaveMoreShotsThanSquares);
L2250:      goto L2890;
        }
        foreach (var shot1 in humanShotSelector.GetShots(_io))
        {
            if (computerGrid.IsHit(shot1, turnNumber, out var shipName))
            {
                _io.Write(Strings.YouHit(shipName));
            }
        }
L2620:  if (startResponse == "YES") { goto L2670; }
L2640:  turnNumber++;
L2660:  _io.Write(Strings.Turn(turnNumber));
L2670:  numberOfShots = computerGrid.Ships.Sum(s => s.Shots);
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
L3200:  if (shotCount==numberOfShots) { goto L3380; }
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
L3390:  for (var i = 1; i <= numberOfShots; i++)
        {
L3400:      _io.WriteLine(temp[i]);
        }
L3420:  for (var i = 1; i <= numberOfShots; i++)
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
