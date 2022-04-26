using Poker.Resources;
using static System.StringComparison;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    private class FloatArray
    {
        private float[] _values;

        public FloatArray(float[] values)
        {
            _values = values;
        }

        public float this[float index]
        {
            get => _values[(int)index];
            set => _values[(int)index] = value;
        }
    }

    private readonly FloatArray AA = new(new float[51]);  // 10 DIM A(50),B(15)
    private readonly FloatArray BB = new(new float[16]);
    private float O = 1;  // 90 = 120
    private float C = 200;
    private float S = 200;
    private float P = 0;

    private float B;
    private float M;
    private float T;
    private float D;
    private float G;
    private float I;
    private float N;
    private float U;
    private float Z;
    private float X;
    private float K;
    private float V;
    private float Q;

    private string JS;
    private string KS;
    private string HS;
    private string IS;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    private float FNA() => (int)_random.NextFloat(10); // 20 DEF FNAA[X]=(int)(10*_random.NextFloat())

    private float FNB(float x) => x - 100 * (int)(x / 100); // 30 DEF FNB(X)=X-100*(int)(X/100)

    internal void Play()
    {
        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);

_90:    O=1;
_100:   C=200;
_110:   S=200;

        while(PlayHand());
    }

    internal bool PlayHand()
    {
_120:   P=0;
_140:   _io.WriteLine();
_150:   if (C<=5) { Line_3670(); return false; }
_160:   _io.WriteLine("The ante is $5.  I will deal:");
_170:   _io.WriteLine();
_180:   if (S>5) { goto _200; }
_190:   if (Line_3830()) { return false; }
_200:   P=P+10;
_210:   S=S-5;
_220:   C=C-5;
_230:   for (Z=1; Z <= 10; Z++)
        {
_240:       Line_1740();
        }
_260:   _io.WriteLine("Your hand:");
_270:   N=1;
_280:   Line_1850();
_290:   N=6;
_300:   I=2;
_310:   Line_2170();
_320:   _io.WriteLine();
_330:   if (I!=6) { goto _470; }
_340:   if (FNA()<=7) { goto _370; }
_350:   X=11100;
_360:   goto _420;
_370:   if (FNA()<=7) { goto _400; }
_380:   X=11110;
_390:   goto _420;
_400:   if (FNA()>=1) { goto _450; }
_410:   X=11111;
_420:   I=7;
_430:   Z=23;
_440:   goto _580;
_450:   Z=1;
_460:   goto _510;
_470:   if (U>=13) { goto _540; }
_480:   if (FNA()>=2) { goto _500; }
_490:   goto _420;
_500:   Z=0;
_510:   K=0;
_520:   _io.WriteLine("I check.");
_530:   goto _620;
_540:   if (U<=16) { goto _570; }
_550:   Z=2;
_560:   if (FNA()>=1) { goto _580; }
_570:   Z=35;
_580:   V=Z+FNA();
_590:   if (Line_3480()) { return false; }
_600:   _io.WriteLine($"I'll open with ${V}");
_610:   K=V;
_620:   if (Line_3050()) { return false; }
_630:   var response = Line_650();
        if (response.HasValue) { return response.Value; }
_640:   goto _820;

        bool? Line_650()
        {
_650:       if (I!=3) { return Line_760(); }
_660:       _io.WriteLine();
            return Line_670();
        }

        bool? Line_670()
        {
_670:       _io.WriteLine("I win.");
_680:       C=C+P;
            return Line_690();
        }

        bool? Line_690()
        {
_690:       _io.WriteLine($"Now I have ${C}and you have ${S}");
_700:       _io.Write("Do you wish to continue");
_710:       HS = _io.ReadString("");
_720:       if (HS.Equals("YES", InvariantCultureIgnoreCase)) { return true; }
_730:       if (HS.Equals("NO", InvariantCultureIgnoreCase)) { return false; }
_740:       _io.WriteLine("Answer Yes or No, please.");
_750:       goto _700;
        }

        bool? Line_760()
        {
_760:       if (I!=4) { return Line_810(); }
_770:       _io.WriteLine();
            return Line_780();
        }

        bool? Line_780()
        {
_780:       _io.WriteLine("You win.");
_790:       S=S+P;
_800:       return Line_690();
        }

        bool? Line_810()
        {
_810:       return null;
        }

_820:   _io.WriteLine();
_830:   _io.Write("Now we draw -- How many cards do you want");
_840:   T = _io.ReadNumber("");
_850:   if (T==0) { goto _980; }
_860:   Z=10;
_870:   if (T<4) { goto _900; }
_880:   _io.WriteLine("You can't draw more than three cards.");
_890:   goto _840;
_900:   _io.WriteLine("What are their numbers:");
_910:   for (Q=1; Q <= T; Q++)
        {
_920:       U = _io.ReadNumber("");
_930:       Line_1730();
        }
_950:   _io.WriteLine("Your new hand:");
_960:   N=1;
_970:   Line_1850();
_980:   Z=10+T;
_990:   for (U=6; U <= 10; U++)
        {
_1000:      if ((int)(X/Math.Pow(10, U-6))!=10*(int)(X/Math.Pow(10, U-5))) { goto _1020; }
_1010:      Line_1730();
_1020:      ;
        }
_1030:   _io.WriteLine();
_1040:   _io.Write($"I am taking{Z-10-T}card");
_1050:   if (Z==11+T) { goto _1090; }
_1060:   _io.WriteLine("s");
_1070:   _io.WriteLine();
_1080:   goto _1100;
_1090:   _io.WriteLine();
_1100:   N=6;
_1110:   V=I;
_1120:   I=1;
_1130:   Line_2170();
_1140:   B=U;
_1150:   M=D;
_1160:   if (V!=7) { goto _1190; }
_1170:   Z=28;
_1180:   goto _1330;
_1190:   if (I!=6) { goto _1220; }
_1200:   Z=1;
_1210:   goto _1330;
_1220:   if (U>=13) { goto _1270; }
_1230:   Z=2;
_1240:   if (FNA()!=6) { goto _1260; }
_1250:   Z=19;
_1260:   goto _1330;
_1270:   if (U>=16) { goto _1320; }
_1280:   Z=19;
_1290:   if (FNA()!=8) { goto _1310; }
_1300:   Z=11;
_1310:   goto _1330;
_1320:   Z=2;
_1330:   K=0;
_1340:   if (Line_3050()) { return false; }
_1350:   if (T!=.5) { goto _1450; }
_1360:   if (V==7) { goto _1400; }
_1370:   if (I!=6) { goto _1400; }
_1380:   _io.WriteLine("I'll check");
_1390:   goto _1460;
_1400:   V=Z+FNA();
_1410:   if (Line_3480()) { return false; }
_1420:   _io.WriteLine($"I'll bet ${V}");
_1430:   K=V;
_1440:   if (Line_3060()) { return false; }
_1450:   response = Line_650();
         if (response.HasValue) { return response.Value; }
_1460:   _io.WriteLine();
_1470:   _io.WriteLine("Now we compare hands:");
_1480:   JS=HS;
_1490:   KS=IS;
_1500:   _io.WriteLine("My hand:");
_1510:   N=6;
_1520:   Line_1850();
_1530:   N=1;
_1540:   Line_2170();
_1550:   _io.WriteLine();
_1560:   _io.Write("You have ");
_1570:   K=D;
_1580:   Line_3690();
_1590:   HS=JS;
_1600:   IS=KS;
_1610:   K=M;
_1620:   _io.Write("and I have ");
_1630:   Line_3690();
_1640:   if (B>U) { return Line_670().Value; }
_1650:   if (U>B) { return Line_780().Value; }
_1660:   if (HS=="A Flus") { goto _1700; }
_1662:   if (FNB(M)<FNB(D)) { return Line_780().Value; }
_1664:   if (FNB(M)>FNB(D)) { return Line_670().Value; }
_1670:   _io.WriteLine("The hand is drawn.");
_1680:   _io.WriteLine($"All ${P}remains in the pot.");
_1690:   goto _140;
_1700:   if (FNB(M)>FNB(D)) { return Line_670().Value; }
_1710:   if (FNB(D)>FNB(M)) { return Line_780().Value; }
_1720:   goto _1670;

        void Line_1730()
        {
_1730:      Z=Z+1;
            Line_1740();
        }

        void Line_1740()
        {
_1740:      AA[Z]=100*(int)(4*_random.NextFloat())+(int)(100*_random.NextFloat());
_1750:      if ((int)(AA[Z]/100)>3) { goto _1740; }
_1760:      if (AA[Z]-100*(int)(AA[Z]/100)>12) { goto _1740; }
_1765:      if (Z==1) { goto _1840; }
_1770:      for (K=1; K <= Z-1; K++)
            {
_1780:          if (AA[Z]==AA[K]) { goto _1740; }
            }
_1800:      if (Z<=10) { goto _1840; }
_1810:      N=AA[U];
_1820:      AA[U]=AA[Z];
_1830:      AA[Z]=N;
_1840:      return;
        }

        void Line_1850()
        {
_1850:      for (Z = N; Z <= N+4; Z++)
            {
_1860:          _io.Write($"{Z}--  ");
_1870:          Line_1950();
_1880:          _io.Write(" of");
_1890:          Line_2070();
_1900:          if (Z/2!=(int)(Z/2)) { goto _1920; }
_1910:          _io.WriteLine();
_1920:          ;
            }
_1930:      _io.WriteLine();
_1940:      return;
        }

        void Line_1950()
        {
_1950:      K=FNB(AA[Z]);
            Line_1960();
        }

        void Line_1960()
        {
_1960:      if (K!=9) { goto _1980; }
_1970:      _io.Write("Jack");
_1980:      if (K!=10) { goto _2000; }
_1990:      _io.Write("Queen");
_2000:      if (K!=11) { goto _2020; }
_2010:      _io.Write("King");
_2020:      if (K!=12) { goto _2040; }
_2030:      _io.Write("Ace");
_2040:      if (K>=9) { goto _2060; }
_2050:      _io.Write(K+2);
_2060:      return;
        }

        void Line_2070()
        {
_2070:      K=(int)(AA[Z]/100);
            Line_2080();
        }

        void Line_2080()
        {
_2080:      if (K!=0) { goto _2100; }
_2090:      _io.Write(" Clubs");
_2100:      if (K!=1) { goto _2120; }
_2110:      _io.Write(" Diamonds");
_2120:      if (K!=2) { goto _2140; }
_2130:      _io.Write(" Hearts");
_2140:      if (K!=3) { goto _2160; }
_2150:      _io.Write(" Spades");
_2160:      return;
        }

        void Line_2170()
        {
_2170:      U=0;
_2180:      for (Z=N; Z <= N+4; Z++)
            {
_2190:         BB[Z]=FNB(AA[Z]);
_2200:         if (Z==N+4) { goto _2230; }
_2210:         if ((int)(AA[Z]/100)!=(int)(AA[Z+1]/100)) { goto _2230; }
_2220:         U=U+1;
_2230:         ;
            }
_2240:      if (U!=4) { goto _2310; }
_2250:      X=11111;
_2260:      D=AA[N];
_2270:      HS="A Flus";
_2280:      IS="h in";
_2290:      U=15;
_2300:      return;
_2310:      for (Z=N; Z <= N+3; Z++)
            {
_2320:          for (K=Z+1; K <= N+4; K++)
                {
_2330:              if (BB[Z]<=BB[K]) { goto _2390; }
_2340:              X=AA[Z];
_2350:              AA[Z]=AA[K];
_2360:              BB[Z]=BB[K];
_2370:              AA[K]=X;
_2380:              BB[K]=AA[K]-100*(int)(AA[K]/100);
_2390:              ;
                }
            }
_2410:      X=0;
_2420:      for (Z=N; Z <= N+3; Z++)
            {
_2430:          if (BB[Z]!=BB[Z+1]) { goto _2470; }
_2440:          X=X+11*(float)Math.Pow(10, Z-N);
_2450:          D=AA[Z];
_2460:          Line_2760();
_2470:          ;
            }
_2480:      if (X!=0) { goto _2620; }
_2490:      if (BB[N]+3!=BB[N+3]) { goto _2520; }
_2500:      X=1111;
_2510:      U=10;
_2520:      if (BB[N+1]+3!=BB[N+4]) { goto _2620; }
_2530:      if (U!=10) { goto _2600; }
_2540:      U=14;
_2550:      HS="Straig";
_2560:      IS="ht";
_2570:      X=11111;
_2580:      D=AA[N+4];
_2590:      return;
_2600:      U=10;
_2610:      X=11110;
_2620:      if (U>=10) { goto _2690; }
_2630:      D=AA[N+4];
_2640:      HS="Schmal";
_2650:      IS="tz, ";
_2660:      U=9;
_2670:      X=11000;
_2680:      goto _2740;
_2690:      if (U!=10) { goto _2720; }
_2700:      if (I==1) { goto _2740; }
_2710:      goto _2750;
_2720:      if (U>12) { goto _2750; }
_2730:      if (FNB(D)>6) { goto _2750; }
_2740:      I=6;
_2750:      return;
        }

        void Line_2760()
        {
_2760:      if (U>=11) { goto _2810; }
_2770:      U=11;
_2780:      HS="A Pair";
_2790:      IS=" OF ";
_2800:      return;
_2810:      if (U!=11) { goto _2910; }
_2820:      if (BB[Z]!=BB[Z-1]) { goto _2870; }
_2830:      HS="Three";
_2840:      IS=" ";
_2850:      U=13;
_2860:      return;
_2870:      HS="Two P";
_2880:      IS="air, ";
_2890:      U=12;
_2900:      return;
_2910:      if (U>12) { goto _2960; }
_2920:      U=16;
_2930:      HS="Full H";
_2940:      IS="ouse, ";
_2950:      return;
_2960:      if (BB[Z]!=BB[Z-1]) { goto _3010; }
_2970:      U=17;
_2980:      HS="Four";
_2990:      IS=" ";
_3000:      return;
_3010:      U=16;
_3020:      HS="Full H";
_3030:      IS="ouse, ";
_3040:      return;
        }

        bool Line_3050()
        {
_3050:      G=0;
            return Line_3060();
        }

        bool Line_3060()
        {
_3060:      _io.WriteLine();
            _io.Write("What is your bet");
_3070:      T = _io.ReadNumber("");
_3080:      if ((T-(int)T)==0) { goto _3140; }
_3090:      if (K!=0) { goto _3120; }
_3100:      if (G!=0) { goto _3120; }
_3110:      if (T==.5) { return Line_3410(); }
_3120:      _io.WriteLine("No small change, please.");
_3130:      goto _3060;
_3140:      if (S-G-T>=0) { goto _3170; }
_3150:      if (Line_3830()) { return true; }
_3160:      goto _3060;
_3170:      if (T!=0) { goto _3200; }
_3180:      I=3;
_3190:      return Line_3380();
_3200:      if (G+T>=K) { goto _3230; }
_3210:      _io.WriteLine("If you can't see my bet, then fold.");
_3220:      goto _3060;
_3230:      G=G+T;
_3240:      if (G==K) { return Line_3380(); }
_3250:      if (Z!=1) { return Line_3420(); }
_3260:      if (G>5) { goto _3300; }
_3270:      if (Z>=2) { return Line_3350(); }
_3280:      V=5;
_3290:      return Line_3420();
_3300:      if (Z==1) { goto _3320; }
_3310:      if (T<=25) { return Line_3350(); }
_3320:      I=4;
_3330:      _io.WriteLine("I fold.");
_3340:      return false;
        }

        bool Line_3350()
        {
_3350:      if (Z==2) { return Line_3430(); }
            return Line_3360();
        }

        bool Line_3360()
        {
_3360:      _io.WriteLine("I'll see you.");
_3370:      K=G;
            return Line_3380();
        }

        bool Line_3380()
        {
_3380:      S=S-G;
_3390:      C=C-K;
_3400:      P=P+G+K;
            return Line_3410();
        }

        bool Line_3410()
        {
_3410:      return false;
        }

        bool Line_3420()
        {
_3420:      if (G>3*Z) { return Line_3350(); }
            return Line_3430();
        }

        bool Line_3430()
        {
_3430:      V=G-K+FNA();
_3440:      if (Line_3480()) { return true; }
_3450:      _io.WriteLine($"I'll see you, and raise you{V}");
_3460:      K=G+V;
_3470:      return Line_3060();
        }

        bool Line_3480()
        {
_3480:      if (C-G-V>=0) { goto _3660; }
_3490:      if (G!=0) { goto _3520; }
_3500:      V=C;
_3510:      return false;
_3520:      if (C-G>=0) { return Line_3360(); }
_3530:      if (O % 2 != 0) { goto _3600; }
_3540:      _io.Write("Would you like to buy back your watch for $50");
_3550:      JS = _io.ReadString("");
_3560:      if (JS.StartsWith("N", InvariantCultureIgnoreCase)) { goto _3600; }
_3570:      C=C+50;
_3580:      O=O/2;
_3590:      return false;
_3600:      if (O % 3!= 0) { return Line_3670(); }
_3610:      _io.Write("Would you like to buy back your tie tack for $50");
_3620:      JS = _io.ReadString("");
_3630:      if (JS.StartsWith("N", InvariantCultureIgnoreCase)) { return Line_3670(); }
_3640:      C=C+50;
_3650:      O=O/3;
_3660:      return false;
        }

        bool Line_3670()
        {
_3670:      _io.WriteLine("I'm busted.  Congratulations!");
_3680:      return true;  // STOP
        }

        void Line_3690()
        {
_3690:      _io.Write($"{HS}{IS}");
_3700:      if (HS!="A FLUS") { goto _3750; }
_3710:      K=K/100;
_3720:      Line_2080();
_3730:      _io.WriteLine();
_3740:      return;
_3750:      K=FNB(K);
_3760:      Line_1960();
_3770:      if (HS=="Schmal") { goto _3790; }
_3780:      if (HS!="Straig") { goto _3810; }
_3790:      _io.WriteLine(" High");
_3800:      return;
_3810:      _io.WriteLine("'s");
_3820:      return;
        }

        bool Line_3830()
        {
_3830:      _io.WriteLine();
_3840:      _io.WriteLine("You can't bet with what you haven't got.");
_3850:      if (O/2 == (int)(O/2)) { goto _3970; }
_3860:      _io.Write("Would you like to sell your watch");
_3870:      JS = _io.ReadString("");
_3880:      if (JS.StartsWith("N", InvariantCultureIgnoreCase)) { goto _3970; }
_3890:      if (FNA()>=7) { goto _3930; }
_3900:      _io.WriteLine("I'll give you $75 for it.");
_3910:      S=S+75;
_3920:      goto _3950;
_3930:      _io.WriteLine("That's a pretty crummy watch - I'll give you $25.");
_3940:      S=S+25;
_3950:      O=O*2;
_3960:      return false;
_3970:      if (O/3 != (int)(O/3)) { goto _4090; }
_3980:      _io.WriteLine("Will you part with that diamond tie tack");
_3990:      JS = _io.ReadString("");
_4000:      if (JS.StartsWith("N", InvariantCultureIgnoreCase)) { goto _4080; }
_4010:      if (FNA()>=6) { goto _4050; }
_4020:      _io.WriteLine("You are now $100 richer.");
_4030:      S=S+100;
_4040:      goto _4070;
_4050:      _io.WriteLine("It's paste.  $25.");
_4060:      S=S+25;
_4070:      O=O*3;
_4080:      return false;
_4090:      _io.WriteLine("Your wad is shot.  So long, sucker!");
_4100:      return true;
        }
    }
}