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

L1040:  var AA = new float[11,11];
        var BA = new float[11,11];
        var CA = new int[8];
        var DA = new int[8];
        var EA = new int[13];
        var FA = new int[13];
        var GA = new int[13];
        var HA = new float[13];
        var KA = new int[11,11];
L1050:  var Z8 = 0;
L1060:  for (var W = 1; W <= 12; W++)
        {
L1070:      EA[W] = -1;
L1080:      HA[W] = -1;
        }
L1100:  for (var X = 1; X <= 10; X++)
        {
L1110:      for (var Y = 1; Y <= 10; Y++)
            {
L1120:          BA[X,Y] = 0;
            }
        }
L1150:  for (var X = 1; X <= 12; X++)
        {
L1160:      FA[X]=0;
L1170:      GA[X]=0;
        }
L1190:  for (var X = 1; X <= 10; X++)
        {
L1200:      for (var Y = 1; Y <= 10; Y++)
            {
L1210:          AA[X,Y]=0;
            }
        }
L1240:  for (var K = 4; K >= 1; K--)
        {
L1250:      var U6=0;
L1260:      var (X, Y, V, V2) = L2910();
L1270:      int FNA(int K) => (5-K)*3-2*K/4+Math.Sign(K-1)-1;
L1280:      int FNB(int K) => K+K/4-Math.Sign(K-1);
L1290:      if (V+V2+V*V2 == 0) { goto L1260; }
L1300:      if (Y+V*FNB(K)>10) { goto L1260; }
L1310:      if (Y+V*FNB(K)<1) { goto L1260; }
L1320:      if (X+V2*FNB(K)>10) { goto L1260; }
L1330:      if (X+V2*FNB(K)<1) { goto L1260; }
L1340:      U6=U6+1;
L1350:      if (U6>25) { goto L1190; }
L1360:      for (var Z = 0; Z <= FNB(K); Z++)
            {
L1370:          FA[Z+FNA(K)]=X+V2*Z;
L1380:          GA[Z+FNA(K)]=Y+V*Z;
            }
L1400:      var U8=FNA(K);
L1405:      if (U8>U8+FNB(K)) { goto L1460; }
L1410:      for (var Z2 =  U8; Z2 <= U8+FNB(K); Z2++)
            {
L1415:          if (U8<2) { continue; }
L1420:          for (var Z3 = 1; Z3 <= U8-1; Z3++)
                {
L1430:              if (Math.Sqrt((FA[Z3]-FA[Z2])*(FA[Z3]-FA[Z2]) + (GA[Z3]-GA[Z2])*(GA[Z3]-GA[Z2])) < 3.59) { goto L1260; }
                }
            }
L1460:      for (var Z = 0; Z <= FNB(K); Z++)
            {
L1470:          AA[FA[Z+U8],GA[Z+U8]]=.5F+Math.Sign(K-1)*(K-1.5F);
            }
        }
L1500:  _io.WriteLine("ENTER COORDINATES FOR...");
L1510:  _io.WriteLine("BATTLESHIP");
L1520:  for (var X = 1; X <= 5; X++)
        {
            var (Y, Z) = _io.Read2Numbers("");
            BA[(int)Y, (int)Z] = 3;
L1540:      BA[(int)Y, (int)Z] = 3;
        }
L1560:  _io.WriteLine("CRUISER");
L1570:  for (var X = 1; X <= 3; X++)
        {
            var (Y, Z) = _io.Read2Numbers("");
L1590:      BA[(int)Y, (int)Z]=2;
        }
L1610:  _io.WriteLine("DESTROYER<A>");
L1620:  for (var X = 1; X <= 2; X++)
        {
            var (Y, Z) = _io.Read2Numbers("");
L1640:      BA[(int)Y, (int)Z]=1;
        }
L1660:  _io.WriteLine("DESTROYER<B>");
L1670:  for (var X = 1; X <= 2; X++)
        {
            var (Y, Z) = _io.Read2Numbers("");
L1690:      BA[(int)Y, (int)Z]=.5F;
        }
L1710:  var JS = _io.ReadString("DO YOU WANT TO START");
L1730:  if (JS != "WHERE ARE YOUR SHIPS?") { goto L1890; }
L1740:  _io.WriteLine("BATTLESHIP");
L1750:  for (var Z = 1; Z <= 5; Z++)
        {
L1760:      _io.WriteLine($" {FA[Z]}  {GA[Z]} ");
        }
L1780:  _io.WriteLine("CRUISER");
L1790:  _io.WriteLine($" {FA[6]}  {GA[6]} ");
L1800:  _io.WriteLine($" {FA[7]}  {GA[7]} ");
L1810:  _io.WriteLine($" {FA[8]}  {GA[8]} ");
L1820:  _io.WriteLine("DESTROYER<A>");
L1830:  _io.WriteLine($" {FA[9]}  {GA[9]} ");
L1840:  _io.WriteLine($" {FA[10]}  {GA[10]} ");
L1850:  _io.WriteLine("DESTROYER<B>");
L1860:  _io.WriteLine($" {FA[11]}  {GA[11]} ");
L1870:  _io.WriteLine($" {FA[12]}  {GA[12]} ");
L1880:  goto L1710;
L1890:  var C=0;
L1900:  var KS = _io.ReadString("DO YOU WANT TO SEE MY SHOTS");
L1920:  _io.WriteLine();
L1930:  if (JS != "YES") { goto L2620; }
L1950:  if (JS != "YES") { goto L1990; }
L1960:  C=C+1;
L1970:  _io.WriteLine();
L1980:  _io.WriteLine($"TURN {C}");
L1990:  var A=0;
L2000:  for (var W = .5F; W <= 3; W += .5F)
        {
L2010:      for (var X = 1; X <= 10; X++)
            {
L2020:          for (var Y = 1; Y <= 10; Y++)
                {
L2030:              if (BA[X,Y] == W) { goto L2070; }
                }
            }
L2060:      continue;
L2070:      A=A+(int)(W+.5F);
        }
L2090:  for (var W = 1; W <= 7; W++)
        {
L2100:      CA[W] = 0;
L2110:      DA[W] = 0;
L2120:      FA[W] = 0;
L2130:      GA[W] = 0;
        }
L2150:  var P3=0;
L2160:  for (var X = 1; X <= 10; X++)
        {
L2170:      for (var Y = 1; Y <= 10; Y++)
            {
L2180:          if (AA[X,Y]>10) { continue; }
L2190:          P3=P3+1;
            }
        }
L2220:  _io.WriteLine($"YOU HAVE {A} SHOTS.");
L2230:  if (P3 >= A) { goto L2260; }
L2240:  _io.WriteLine("YOU HAVE MORE SHOTS THAN THERE ARE BLANK SQUARES.");
L2250:  goto L2890;
L2260:  if (A != 0) { goto L2290; }
L2270:  _io.WriteLine("I HAVE WON.");
L2280:  return;
L2290:  for (var W = 1; W <= A; W++)
        {
L2300:      var (X, Y) = _io.Read2Numbers("");
L2310:      if (X != (int)X) { goto L2370; }
L2320:      if (X > 10) { goto L2370; }
L2330:      if (X < 1) { goto L2370; }
L2340:      if (Y != (int)Y) { goto L2370; }
L2350:      if (Y > 10) { goto L2370; }
L2360:      if (Y >= 1) { goto L2390; }
L2370:      _io.WriteLine("ILLEGAL, ENTER AGAIN.");
L2380:      goto L2300;
L2390:      if (AA[(int)X,(int)Y]>10) 
            { 
                _io.WriteLine($"YOU SHOT THERE BEFORE ON TURN {AA[(int)X,(int)Y]-10}");
                goto L2300;
            }
L2400:      CA[W]=(int)X;
L2410:      DA[W]=(int)Y;
        }
L2460:  for (var W = 1; W <= A; W++)
        {
L2470:      if (AA[CA[W],DA[W]] == 3) 
            { 
                _io.WriteLine("YOU HIT MY BATTLESHIP.");
                goto L2510;
            }
L2480:      if (AA[CA[W],DA[W]] == 2) 
            { 
                _io.WriteLine("YOU HIT MY CRUISER.");
                goto L2510; 
            }
L2490:      if (AA[CA[W],DA[W]] == 1) 
            { 
                _io.WriteLine("YOU HIT MY DESTROYER<A>.");
                goto L2510; 
            }
L2500:      if (AA[CA[W],DA[W]] == .5F) 
            { 
                
                _io.WriteLine("YOU HIT MY DESTROYER<B>.");
                goto L2510; 
            }
L2510:      AA[CA[W],DA[W]] = 10+C;
        }
L2620:  A = 0;
L2630:  if (JS == "YES") { goto L2670; }
L2640:  C = C + 1;
L2650:  _io.WriteLine();
L2660:  _io.WriteLine($"TURN {C}");
L2670:  A = 0;
L2680:  for (var W = .5F; W <= 3; W += .5F)
        {
L2690:      for (var X = 1; X <= 10; X++)
            {
L2700:          for (var Y = 1; Y <= 10; Y++)
                {
L2710:              if (AA[X,Y] == W) { goto L2750; }
                }
            }
L2740:      continue;
L2750:      A = A + (int)(W+.5F);
        }
L2770:  P3=0;
L2780:  for (var X = 1; X <= 10; X++)
        {
L2790:      for (var Y = 1; Y <= 10; Y++)
            {
L2800:          if (AA[X,Y]>10) { continue; }
L2810:          P3=P3+1;
            }
        }
L2840:  _io.WriteLine($"I HAVE {A} SHOTS.");
L2850:  if (P3>A) { goto L2880; }
L2860:  _io.WriteLine("I HAVE MORE SHOTS THAN BLANK SQUARES.");
L2870:  goto L2270;
L2880:  if (A != 0) { goto L2960; }
L2890:  _io.WriteLine("YOU HAVE WON.");
L2900:  return;

        (int, int, int, int) L2910()
        {
            var X = _random.Next(1, 11);
            var Y = _random.Next(1, 11);
            var V = _random.Next(-1, 2);
            var V2 = _random.Next(-1, 2);

            return (X, Y, V, V2);
        }

L2960:  for (var W = 1; W <= 12; W++)
        {
L2970:      if (HA[W]>0) { goto L3800; }
        }
L3000:  var w=0;
L3010:  var R3=0;
L3020:  var (x, y, v, v2) = L2910();
L3030:  //RESTORE
        var index = 0;
L3040:  var R2=0;
L3050:  R3=R3+1;
L3060:  if (R3>100) { goto L3010; }
L3070:  if (x>10) { goto L3110; }
L3080:  if (x>0) { goto L3120; }
L3090:  x = 1 + (int)(_random.NextFloat() * 2.5F);
L3100:  goto L3120;
L3110:  x = 10 - (int)(_random.NextFloat() * 2.5F);
L3120:  if (y>10) { goto L3160; }
L3130:  if (y>0) { goto L3270; }
L3140:  y=1+(int)(_random.NextFloat() * 2.5F);
L3150:  goto L3270;
L3160:  y=10-(int)(_random.NextFloat() * 2.5F);
L3170:  goto L3270;
L3180:  FA[w]=x;
L3190:  GA[w]=y;
L3200:  if (w==A) { goto L3380; }
L3210:  if (R2==6) { goto L3030; }
L3240:  //DATA 1,1,-1,1,1,-3,1,1,0,2,-1,1
        var data = new[] { 1,1,-1,1,1,-3,1,1,0,2,-1,1 };
L3220:  //READ X1,Y1
        var (X1, Y1) = (data[index++], data[index++]);
L3230:  R2=R2+1;
L3250:  x=x+X1;
L3260:  y=y+Y1;
L3270:  if (x>10) { goto L3210; }
L3280:  if (x<1) { goto L3210; }
L3290:  if (y>10) { goto L3210; }
L3300:  if (y<1) { goto L3210; }
L3310:  if (BA[x,y]>10) { goto L3210; }
L3320:  for (var Q9 = 1; Q9 <= w; Q9++)
        {
L3330:      if (FA[Q9] != x) { continue; }
L3340:      if (GA[Q9] == y) { goto L3210; }
        }
L3360:  w=w+1;
L3370:  goto L3180;
L3380:  if (KS != "YES") { goto L3420; }
L3390:  for (var Z5 = 1; Z5 <= A; Z5++)
        {
L3400:      _io.WriteLine($" {FA[Z5]}  {GA[Z5]}");
        }
L3420:  for (var W = 1; W <= A; W++)
        {
L3430:      if (BA[FA[W],GA[W]] == 3) 
            { 
                _io.WriteLine("I HIT YOUR BATTLESHIP");
            }
            else if (BA[FA[W],GA[W]] == 2) 
            { 
                _io.WriteLine("I HIT YOUR CRUISER");
            }
            else if (BA[FA[W],GA[W]] == 1) 
            { 
                _io.WriteLine("I HIT YOUR DESTROYER<A>");
            }
            else if (BA[FA[W],GA[W]] == .5F) 
            { 
                _io.WriteLine("I HIT YOUR DESTROYER<B>");
            }
            else
            {
                BA[FA[W],GA[W]]=10+C;
                continue;
            }
L3570:      for (var Q = 1; Q <= 12; Q++)
            {
L3580:          if (EA[Q] != -1) { continue; }
L3590:          EA[Q]=10+C;
L3600:          HA[Q]=BA[FA[W],GA[W]];
L3610:          var M3=0;
L3620:          for (var M2 = 1; M2 <= 12; M2++)
                {
L3630:              if (HA[M2] != HA[Q]) { continue; }
L3640:              M3=M3+1;
                }
L3660:          if (M3 != (int)(HA[Q]+.5F)+1+(int)(HA[Q]+.5F)/3) { goto L3470; }
L3670:          for (var M2 = 1; M2 <= 12; M2++)
                {
L3680:              if (HA[M2] != HA[Q]) { continue; }
L3690:              EA[M2] = -1;
L3700:              HA[M2] = -1;
                }
L3720:          goto L3470;
            }
L3740:      _io.WriteLine("PROGRAM ABORT:");
L3750:      for (var Q = 1; Q <= 12; Q++)
            {
L3760:          _io.WriteLine($"EA[{Q}] = {EA[Q]}");
L3770:          _io.WriteLine($"HA[{Q}] = {HA[Q]}");
            }
            return;
L3470:      BA[FA[W],GA[W]]=10+C;
        }
L3490:  goto L1950;
L3800:  //REM************************USINGEARRAY
L3810:  for (var R = 1; R <= 10; R++)
        {
L3820:      for (var S = 1; S <= 10; S++)
            {
                KA[R,S]=0;
            }
        }
L3860:  for (var U = 1; U <= 12; U++)
        {
L3870:      if (EA[U]<10) { continue; }
L3880:      for (var R = 1; R <= 10; R++)
            {
L3890:          for (var S = 1; S <= 10; S++)
                {
L3900:              if (BA[R,S]<10) { goto L3930; }
L3910:              KA[R,S]=-10000000;
L3920:              continue;
L3930:              for (var M = Math.Sign(1-R); M <= Math.Sign(10-R); M++)
                    {
L3940:                  for (var N = Math.Sign(1-S); N <= Math.Sign(10-S); N++)
                        {
L3950:                      if (N+M+N*M==0) { continue; }
L3960:                      if (BA[R+M,S+N] != EA[U]) { continue; }
L3970:                      KA[R,S]=KA[R,S]+EA[U]-S*(int)(HA[U]+.5F);
                        }
                    }
                }
            }
        }
L4030:  for (var R = 1; R <= A; R++)
        {
L4040:      FA[R]=R;
L4050:      GA[R]=R;
        }
L4070:  for (var R = 1; R <= 10; R++)
        {
L4080:      for (var S = 1; S <= 10; S++)
            {
L4090:          var Q9=1;
L4100:          for (var M = 1; M <= A; M++)
                {
L4110:              if (KA[FA[M],GA[M]]>=KA[FA[Q9],GA[Q9]]) { continue; }
L4120:              Q9=M;
                }
L4131:          if (R>A) { goto L4140; }
L4132:          if (R==S) { goto L4210; }
L4140:          if (KA[R,S]<KA[FA[Q9],GA[Q9]]) { goto L4210; }
L4150:          for (var M = 1; M <= A; M++)
                {
L4160:              if (FA[M] != R) { break; }
L4170:              if (GA[M] == S) { goto L4210; }
                }
L4190:          FA[Q9]=R;
L4200:          GA[Q9]=S;
L4210:          R=R;// NoOp -  NEXT S 
            }
        }
L4230:  goto L3380;

    }
}