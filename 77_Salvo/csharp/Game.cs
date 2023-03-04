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

L1040:  var computerGrid = new float[11,11];
        var humanGrid = new float[11,11];
        var shotsX = new int[8];
        var shotsY = new int[8];
        var hitTurnRecord = new int[13];
        var tempX = new int[13];
        var tempY = new int[13];
        var hitShipValue = new float[13];
        var temp = new int[11,11];
L1060:  for (var W = 1; W <= 12; W++)
        {
L1070:      hitTurnRecord[W] = -1;
L1080:      hitShipValue[W] = -1;
        }
L1100:  for (var X = 1; X <= 10; X++)
        {
L1110:      for (var Y = 1; Y <= 10; Y++)
            {
L1120:          humanGrid[X,Y] = 0;
            }
        }
L1150:  for (var X = 1; X <= 12; X++)
        {
L1160:      tempX[X]=0;
L1170:      tempY[X]=0;
        }
L1190:  for (var X = 1; X <= 10; X++)
        {
L1200:      for (var Y = 1; Y <= 10; Y++)
            {
L1210:          computerGrid[X,Y]=0;
            }
        }
L1240:  for (var K = 4; K >= 1; K--)
        {
L1250:      var shipGenerationAttempts=0;
L1260:      var (startX, startY, deltaY, deltaX) = L2910();
L1270:      int GetFirstIndex(int shipNumber) => (5-shipNumber)*3-2*(shipNumber/4)+Math.Sign(shipNumber-1)-1;
L1280:      int GetShipSizeLessOne(int shipNumber) => shipNumber + shipNumber/4 - Math.Sign(shipNumber-1);
L1290:      if (deltaY+deltaX+deltaY*deltaX == 0) { goto L1260; }
L1300:      if (startY+deltaY*GetShipSizeLessOne(K)>10) { goto L1260; }
L1310:      if (startY+deltaY*GetShipSizeLessOne(K)<1) { goto L1260; }
L1320:      if (startX+deltaX*GetShipSizeLessOne(K)>10) { goto L1260; }
L1330:      if (startX+deltaX*GetShipSizeLessOne(K)<1) { goto L1260; }
L1340:      shipGenerationAttempts=shipGenerationAttempts+1;
L1350:      if (shipGenerationAttempts>25) { goto L1190; }
            // determine ship coordinates
L1360:      for (var i = 0; i <= GetShipSizeLessOne(K); i++)
            {
                tempX[i+GetFirstIndex(K)]=startX+deltaX*i;
L1380:          tempY[i+GetFirstIndex(K)]=startY+deltaY*i;
            }
L1400:      var firstIndex=GetFirstIndex(K);
L1405:      if (firstIndex>firstIndex+GetShipSizeLessOne(K)) { goto L1460; } // Can't be true
            // detect proximity to previous ships
            for (var i = firstIndex; i <= firstIndex+GetShipSizeLessOne(K); i++)
            {
L1415:          if (firstIndex<2) { continue; } // Only true for the Battleship
L1420:          for (var j = 1; j <= firstIndex-1; j++)
                {
L1430:              if (Math.Sqrt((tempX[j]-tempX[i])*(tempX[j]-tempX[i]) + (tempY[j]-tempY[i])*(tempY[j]-tempY[i])) < 3.59) { goto L1260; }
                }
            }
            // put ship on board
L1460:      for (var i = 0; i <= GetShipSizeLessOne(K); i++)
            {
L1470:          computerGrid[tempX[i+firstIndex],tempY[i+firstIndex]]=.5F+Math.Sign(K-1)*(K-1.5F);
            }
        }
L1500:  _io.WriteLine("ENTER COORDINATES FOR...");
L1510:  _io.WriteLine("BATTLESHIP");
L1520:  for (var i = 1; i <= 5; i++)
        {
            var (x, y) = _io.Read2Numbers("");
L1540:      humanGrid[(int)x, (int)y] = 3;
        }
L1560:  _io.WriteLine("CRUISER");
L1570:  for (var i = 1; i <= 3; i++)
        {
            var (x, y) = _io.Read2Numbers("");
L1590:      humanGrid[(int)x, (int)y]=2;
        }
L1610:  _io.WriteLine("DESTROYER<A>");
L1620:  for (var i = 1; i <= 2; i++)
        {
            var (x, y) = _io.Read2Numbers("");
L1640:      humanGrid[(int)x, (int)y]=1;
        }
L1660:  _io.WriteLine("DESTROYER<B>");
L1670:  for (var i = 1; i <= 2; i++)
        {
            var (x, y) = _io.Read2Numbers("");
L1690:      humanGrid[(int)x, (int)y]=.5F;
        }
L1710:  var startResponse = _io.ReadString("DO YOU WANT TO START");
L1730:  if (startResponse != "WHERE ARE YOUR SHIPS?") { goto L1890; }
L1740:  _io.WriteLine("BATTLESHIP");
L1750:  for (var i = 1; i <= 5; i++)
        {
L1760:      _io.WriteLine($" {tempX[i]}  {tempY[i]} ");
        }
L1780:  _io.WriteLine("CRUISER");
L1790:  _io.WriteLine($" {tempX[6]}  {tempY[6]} ");
L1800:  _io.WriteLine($" {tempX[7]}  {tempY[7]} ");
L1810:  _io.WriteLine($" {tempX[8]}  {tempY[8]} ");
L1820:  _io.WriteLine("DESTROYER<A>");
L1830:  _io.WriteLine($" {tempX[9]}  {tempY[9]} ");
L1840:  _io.WriteLine($" {tempX[10]}  {tempY[10]} ");
L1850:  _io.WriteLine("DESTROYER<B>");
L1860:  _io.WriteLine($" {tempX[11]}  {tempY[11]} ");
L1870:  _io.WriteLine($" {tempX[12]}  {tempY[12]} ");
L1880:  goto L1710;
L1890:  var turnNumber=0;
L1900:  var seeShotsResponse = _io.ReadString("DO YOU WANT TO SEE MY SHOTS");
L1920:  _io.WriteLine();
L1930:  if (startResponse != "YES") { goto L2620; }
L1950:  if (startResponse != "YES") { goto L1990; }
L1960:  turnNumber=turnNumber+1;
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
L2070:      maxShotCount=maxShotCount+(int)(shipValue+.5F);
        }
L2090:  for (var i = 1; i <= 7; i++)
        {
L2100:      shotsX[i] = 0;
L2110:      shotsY[i] = 0;
L2120:      tempX[i] = 0;
L2130:      tempY[i] = 0;
        }
L2150:  var untriedSquareCount=0;
L2160:  for (var x = 1; x <= 10; x++)
        {
L2170:      for (var y = 1; y <= 10; y++)
            {
L2180:          if (computerGrid[x,y]>10) { continue; }
L2190:          untriedSquareCount=untriedSquareCount+1;
            }
        }
L2220:  _io.WriteLine($"YOU HAVE {maxShotCount} SHOTS.");
L2230:  if (untriedSquareCount >= maxShotCount) { goto L2260; }
L2240:  _io.WriteLine("YOU HAVE MORE SHOTS THAN THERE ARE BLANK SQUARES.");
L2250:  goto L2890;
L2260:  if (maxShotCount != 0) { goto L2290; }
L2270:  _io.WriteLine("I HAVE WON.");
L2280:  return;
L2290:  for (var i = 1; i <= maxShotCount; i++)
        {
L2300:      var (x, y) = _io.Read2Numbers("");
L2310:      if (x != (int)x) { goto L2370; }
L2320:      if (x > 10) { goto L2370; }
L2330:      if (x < 1) { goto L2370; }
L2340:      if (y != (int)y) { goto L2370; }
L2350:      if (y > 10) { goto L2370; }
L2360:      if (y >= 1) { goto L2390; }
L2370:      _io.WriteLine("ILLEGAL, ENTER AGAIN.");
L2380:      goto L2300;
L2390:      if (computerGrid[(int)x,(int)y]>10) 
            { 
                _io.WriteLine($"YOU SHOT THERE BEFORE ON TURN {computerGrid[(int)x,(int)y]-10}");
                goto L2300;
            }
L2400:      shotsX[i]=(int)x;
L2410:      shotsY[i]=(int)y;
        }
L2460:  for (var W = 1; W <= maxShotCount; W++)
        {
L2470:      if (computerGrid[shotsX[W],shotsY[W]] == 3) 
            { 
                _io.WriteLine("YOU HIT MY BATTLESHIP.");
                goto L2510;
            }
L2480:      if (computerGrid[shotsX[W],shotsY[W]] == 2) 
            { 
                _io.WriteLine("YOU HIT MY CRUISER.");
                goto L2510; 
            }
L2490:      if (computerGrid[shotsX[W],shotsY[W]] == 1) 
            { 
                _io.WriteLine("YOU HIT MY DESTROYER<A>.");
                goto L2510; 
            }
L2500:      if (computerGrid[shotsX[W],shotsY[W]] == .5F) 
            { 
                
                _io.WriteLine("YOU HIT MY DESTROYER<B>.");
                goto L2510; 
            }
L2510:      computerGrid[shotsX[W],shotsY[W]] = 10+turnNumber;
        }
L2620:  maxShotCount = 0;
L2630:  if (startResponse == "YES") { goto L2670; }
L2640:  turnNumber = turnNumber + 1;
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
L2750:      maxShotCount = maxShotCount + (int)(shipValue+.5F);
        }
L2770:  untriedSquareCount=0;
L2780:  for (var x = 1; x <= 10; x++)
        {
L2790:      for (var y = 1; y <= 10; y++)
            {
L2800:          if (computerGrid[x,y]>10) { continue; }
L2810:          untriedSquareCount=untriedSquareCount+1;
            }
        }
L2840:  _io.WriteLine($"I HAVE {maxShotCount} SHOTS.");
L2850:  if (untriedSquareCount>maxShotCount) { goto L2880; }
L2860:  _io.WriteLine("I HAVE MORE SHOTS THAN BLANK SQUARES.");
L2870:  goto L2270;
L2880:  if (maxShotCount != 0) { goto L2960; }
L2890:  _io.WriteLine("YOU HAVE WON.");
L2900:  return;

        (int, int, int, int) L2910()
        {
            var startX = _random.Next(1, 11);
            var startY = _random.Next(1, 11);
            var deltaY = _random.Next(-1, 2);
            var deltaX = _random.Next(-1, 2);

            return (startX, startY, deltaY, deltaX);
        }

L2960:  for (var i = 1; i <= 12; i++)
        {
            // if damaged ships
L2970:      if (hitShipValue[i]>0) { goto L3800; }
        }
L3000:  var shotCount=0;
L3010:  var shotAttempts=0;
L3020:  var (shotX, shotY, _, _) = L2910();
L3030:  //RESTORE
        var index = 0;
L3040:  var strategyNumber=0;
L3050:  shotAttempts=shotAttempts+1;
L3060:  if (shotAttempts>100) { goto L3010; }
        // ensure shot is in range
L3070:  if (shotX>10) { goto L3110; }
L3080:  if (shotX>0) { goto L3120; }
L3090:  shotX = 1 + (int)_random.NextFloat(2.5F);
L3100:  goto L3120;
L3110:  shotX = 10 - (int)_random.NextFloat(2.5F);
L3120:  if (shotY>10) { goto L3160; }
L3130:  if (shotY>0) { goto L3270; }
L3140:  shotY=1+(int)_random.NextFloat(2.5F);
L3150:  goto L3270;
L3160:  shotY=10-(int)_random.NextFloat(2.5F);
L3170:  goto L3270;
        // record shot
L3180:  tempX[shotCount]=shotX;
L3190:  tempY[shotCount]=shotY;
L3200:  if (shotCount==maxShotCount) { goto L3380; }
L3210:  if (strategyNumber==6) { goto L3030; }
L3240:  //DATA 1,1,-1,1,1,-3,1,1,0,2,-1,1
        var data = new[] { 1,1,-1,1,1,-3,1,1,0,2,-1,1 };
L3220:  //READ X1,Y1
        var (xOffset, yOffset) = (data[index++], data[index++]);
L3230:  strategyNumber=strategyNumber+1;
L3250:  shotX=shotX+xOffset;
L3260:  shotY=shotY+yOffset;
        // is the shot in range?
L3270:  if (shotX>10) { goto L3210; }
L3280:  if (shotX<1) { goto L3210; }
L3290:  if (shotY>10) { goto L3210; }
L3300:  if (shotY<1) { goto L3210; }
        // have we fired here before
L3310:  if (humanGrid[shotX,shotY]>10) { goto L3210; }
        // have we already selected this shot?
L3320:  for (var i = 1; i <= shotCount; i++)
        {
L3330:      if (tempX[i] != shotX) { continue; }
L3340:      if (tempY[i] == shotY) { goto L3210; }
        }
L3360:  shotCount=shotCount+1;
L3370:  goto L3180;
        // display shots
L3380:  if (seeShotsResponse != "YES") { goto L3420; }
L3390:  for (var i = 1; i <= maxShotCount; i++)
        {
L3400:      _io.WriteLine($" {tempX[i]}  {tempY[i]}");
        }
L3420:  for (var i = 1; i <= maxShotCount; i++)
        {
L3430:      if (humanGrid[tempX[i],tempY[i]] == 3) 
            { 
                _io.WriteLine("I HIT YOUR BATTLESHIP");
            }
            else if (humanGrid[tempX[i],tempY[i]] == 2) 
            { 
                _io.WriteLine("I HIT YOUR CRUISER");
            }
            else if (humanGrid[tempX[i],tempY[i]] == 1) 
            { 
                _io.WriteLine("I HIT YOUR DESTROYER<A>");
            }
            else if (humanGrid[tempX[i],tempY[i]] == .5F) 
            { 
                _io.WriteLine("I HIT YOUR DESTROYER<B>");
            }
            else
            {
                humanGrid[tempX[i],tempY[i]]=10+turnNumber;
                continue;
            }
L3570:      for (var j = 1; j <= 12; j++)
            {
                // record hit
L3580:          if (hitTurnRecord[j] != -1) { continue; }
L3590:          hitTurnRecord[j]=10+turnNumber;
L3600:          hitShipValue[j]=humanGrid[tempX[i],tempY[i]];
                // look for past hits on same ship
L3610:          var shipHits=0;
L3620:          for (var k = 1; k <= 12; k++)
                {
L3630:              if (hitShipValue[k] != hitShipValue[j]) { continue; }
L3640:              shipHits=shipHits+1;
                }
                // if ship is not sunk
L3660:          if (shipHits != (int)(hitShipValue[j]+.5F)+1+(int)(hitShipValue[j]+.5F)/3) { goto L3470; }
                // otherwise, remove ship hit records
L3670:          for (var k = 1; k <= 12; k++)
                {
L3680:              if (hitShipValue[k] != hitShipValue[j]) { continue; }
L3690:              hitTurnRecord[k] = -1;
L3700:              hitShipValue[k] = -1;
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
L3470:      humanGrid[tempX[i],tempY[i]]=10+turnNumber;
        }
L3490:  goto L1950;
L3800:  //REM************************USINGEARRAY
L3810:  for (var x = 1; x <= 10; x++)
        {
L3820:      for (var y = 1; y <= 10; y++)
            {
                temp[x,y]=0;
            }
        }
L3860:  for (var i = 1; i <= 12; i++)
        {
L3870:      if (hitTurnRecord[i]<10) { continue; }
L3880:      for (var x = 1; x <= 10; x++)
            {
L3890:          for (var y = 1; y <= 10; y++)
                {
L3900:              if (humanGrid[x,y]<10) { goto L3930; }
L3910:              temp[x,y]=-10000000;
L3920:              continue;
L3930:              for (var dX = Math.Sign(1-x); dX <= Math.Sign(10-x); dX++)
                    {
L3940:                  for (var dY = Math.Sign(1-y); dY <= Math.Sign(10-y); dY++)
                        {
L3950:                      if (dY+dX+dY*dX==0) { continue; }
L3960:                      if (humanGrid[x+dX,y+dY] != hitTurnRecord[i]) { continue; }
L3970:                      temp[x,y]=temp[x,y]+hitTurnRecord[i]-y*(int)(hitShipValue[i]+.5F);
                        }
                    }
                }
            }
        }
L4030:  for (var i = 1; i <= maxShotCount; i++)
        {
L4040:      tempX[i]=i;
L4050:      tempY[i]=i;
        }
L4070:  for (var x = 1; x <= 10; x++)
        {
L4080:      for (var y = 1; y <= 10; y++)
            {
L4090:          var Q9=1;
L4100:          for (var i = 1; i <= maxShotCount; i++)
                {
L4110:              if (temp[tempX[i],tempY[i]]>=temp[tempX[Q9],tempY[Q9]]) { continue; }
L4120:              Q9=i;
                }
L4131:          if (x>maxShotCount) { goto L4140; }
L4132:          if (x==y) { goto L4210; }
L4140:          if (temp[x,y]<temp[tempX[Q9],tempY[Q9]]) { goto L4210; }
L4150:          for (var M = 1; M <= maxShotCount; M++)
                {
L4160:              if (tempX[M] != x) { break; }
L4170:              if (tempY[M] == y) { goto L4210; }
                }
L4190:          tempX[Q9]=x;
L4200:          tempY[Q9]=y;
L4210:          x=x;// NoOp -  NEXT S 
            }
        }
L4230:  goto L3380;

    }
}