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

    private readonly FloatArray _cards = new(new float[51]);  // 10 DIM A(50),B(15)
    private readonly FloatArray _cardRanks = new(new float[16]);
    private float O = 1;  // 90 = 120
    private float _computerBalance = 200;
    private float _playerBalance = 200;
    private float _pot = 0;

    private float B;
    private float M;
    private float T;
    private float D;
    private float G;
    private float I;
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

    private int Get0To9() => _random.Next(10);

    private static int GetRank(float x) => (int)x % 100;

    private static int GetSuit(float x) => (int)x / 100;

    internal void Play()
    {
        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);

        O=1;
        _computerBalance=200;
        _playerBalance=200;
        while(PlayHand());
    }

    internal bool PlayHand()
    {
        _pot=0;
        while(true)
        {
            _io.WriteLine();
            if (_computerBalance<=5)
            {
                CongratulatePlayer();
                return false;
            }
            _io.WriteLine("The ante is $5.  I will deal:");
            _io.WriteLine();
            if (_playerBalance <= 5 && PlayerCantRaiseFunds()) { return false; }
            _pot += 10;
            _playerBalance -= 5;
            _computerBalance -= 5;
            for (Z=1; Z <= 10; Z++)
            {
                DealCard((int)Z);
            }
            _io.WriteLine("Your hand:");
            DisplayHand(1);
            I=2;
            AnalyzeHand(6);
            _io.WriteLine();
_330:       if (I!=6) { goto _470; }
_340:       if (Get0To9()<=7) { goto _370; }
_350:       X=11100;
_360:       goto _420;
_370:       if (Get0To9()<=7) { goto _400; }
_380:       X=11110;
_390:       goto _420;
_400:       if (Get0To9()>=1) { goto _450; }
_410:       X=11111;
_420:       I=7;
_430:       Z=23;
_440:       goto _580;
_450:       Z=1;
_460:       goto _510;
_470:       if (U>=13) { goto _540; }
_480:       if (Get0To9()>=2) { goto _500; }
_490:       goto _420;
_500:       Z=0;
_510:       K=0;
_520:       _io.WriteLine("I check.");
_530:       goto _620;
_540:       Z = U <= 16 || Get0To9() < 1 ? 35 : 2;
_580:       V=Z+Get0To9();
_590:       if (Line_3480()) { return false; }
_600:       _io.WriteLine($"I'll open with ${V}");
_610:       K=V;
            G = 0;
_620:       if (GetWager()) { return false; }
_630:       var response = IsThereAWinner();
            if (response.HasValue) { return response.Value; }

            _io.WriteLine();
            T = _io.ReadNumber("Now we draw -- How many cards do you want", 3, "You can't draw more than three cards.");
            if (T != 0)
            {
                Z=10;
                _io.WriteLine("What are their numbers:");
                for (Q=1; Q <= T; Q++)
                {
                    U = _io.ReadNumber("");
                    DealCard((int)++Z, (int)U);
                }
                _io.WriteLine("Your new hand:");
                DisplayHand(1);
            }
            Z=10+T;
            for (U=6; U <= 10; U++)
            {
                if ((int)(X/Math.Pow(10, U-6)) == 10*(int)(X/Math.Pow(10, U-5)))
                {
                    DealCard((int)++Z, (int)U);
                }
            }
            _io.WriteLine();
            _io.Write($"I am taking{Z-10-T}card");
            if (Z != 11 + T)
            {
                _io.WriteLine("s");
            }
            _io.WriteLine();
            V=I;
            I=1;
            AnalyzeHand(6);
            B=U;
            M=D;
            if (V == 7)
            {
                Z=28;
            }
            else if (I == 6)
            {
                Z=1;
            }
            else if (U < 13)
            {
                Z = Get0To9() == 6 ? 19 : 2;
            }
            else
            {
                if (U >= 16)
                {
                    Z = 2;
                }
                else
                {
                    Z = Get0To9()==8 ? 11 : 19;
                }
            }
_1330:      K=0;
            G=0;
_1340:      if (GetWager()) { return false; }
_1350:      if (T!=.5) { goto _1450; }
_1360:      if (V==7) { goto _1400; }
_1370:      if (I!=6) { goto _1400; }
_1380:      _io.WriteLine("I'll check");
_1390:      goto _1460;
_1400:      V=Z+Get0To9();
_1410:      if (Line_3480()) { return false; }
_1420:      _io.WriteLine($"I'll bet ${V}");
_1430:      K=V;
_1440:      if (GetWager()) { return false; }
_1450:      response = IsThereAWinner();
            if (response.HasValue) { return response.Value; }
_1460:      _io.WriteLine();
_1470:      _io.WriteLine("Now we compare hands:");
_1480:      JS=HS;
_1490:      KS=IS;
_1500:      _io.WriteLine("My hand:");
_1520:      DisplayHand(6);
_1540:      AnalyzeHand(1);
_1550:      _io.WriteLine();
_1560:      _io.Write("You have ");
_1580:      DisplayHandRank(HS, IS, (int)D);
_1620:      _io.Write("and I have ");
_1630:      DisplayHandRank(JS, KS, (int)M);
            if (B > U || GetRank(M) > GetRank(D)) { return ComputerWins().Value; }
            if (U > B || GetRank(D) > GetRank(M)) { return PlayerWins().Value; }
_1670:       _io.WriteLine("The hand is drawn.");
_1680:       _io.WriteLine($"All ${_pot}remains in the pot.");
        }

        bool? IsThereAWinner()
        {
            if (I!=3)
            {
                if (I!=4) { return null; }
                _io.WriteLine();
                return PlayerWins();
            }
            _io.WriteLine();
            return ComputerWins();
        }

        bool? ComputerWins()
        {
            _io.WriteLine("I win.");
            _computerBalance += _pot;
            return ShouldContinue();
        }

        bool? ShouldContinue()
        {
            _io.WriteLine($"Now I have ${_computerBalance}and you have ${_playerBalance}");
            return _io.ReadYesNo("Do you wish to continue");
        }

        bool? PlayerWins()
        {
            _io.WriteLine("You win.");
            _playerBalance += _pot;
            return ShouldContinue();
        }

        void DealCard(int index, int indexToReplace = 0)
        {
            while(true)
            {
                _cards[index]=100*_random.Next(4) + _random.Next(100);
                if (GetSuit(_cards[index]) > 3) { continue; }
                if (GetRank(_cards[index]) > 12) { continue; }
                if (index==1) { break; }
                var matchFound = false;
                for (K=1; K <= index-1; K++)
                {
                    if (_cards[index]==_cards[K])
                    {
                        matchFound = true;
                        break;
                    }
                }
                if (!matchFound) { break; }
            }
            if (index > 10)
            {
                (_cards[indexToReplace], _cards[index]) = (_cards[index], _cards[indexToReplace]);
            }
            return;
        }

        void DisplayHand(int firstCard)
        {
            for (Z = firstCard; Z <= firstCard+4; Z++)
            {
                var card = _cards[Z];
                _io.Write($"{Z}--  ");
                _io.Write(GetRankName(GetRank(card)));
                _io.Write(" of");
                _io.Write(GetSuitName(GetSuit(card)));
                if (Z % 2 == 0)
                {
                    _io.WriteLine();
                }
            }
            _io.WriteLine();
            return;
        }

        string GetRankName(int rank) => rank switch
        {
            9 => "Jack",
            10 => "Queen",
            11 => "King",
            12 => "Ace",
            _ => (rank + 2).ToString()
        };


        string GetSuitName(int suitNumber) => suitNumber switch
        {
            0 => " Clubs",
            1 => " Diamonds",
            2 => " Hearts",
            3 => " Spades",
        };


        void AnalyzeHand(int firstCard)
        {
_2170:      U=0;
_2180:      for (Z=firstCard; Z <= firstCard+4; Z++)
            {
_2190:         _cardRanks[Z]=GetRank(_cards[Z]);
_2200:         if (Z==firstCard+4) { goto _2230; }
_2210:         if (GetSuit(_cards[Z]) != GetSuit(_cards[Z+1])) { goto _2230; }
_2220:         U++;
_2230:         ;
            }
_2240:      if (U!=4) { goto _2310; }
_2250:      X=11111;
_2260:      D=_cards[firstCard];
_2270:      HS="A Flus";
_2280:      IS="h in";
_2290:      U=15;
_2300:      return;
_2310:      for (Z=firstCard; Z <= firstCard+3; Z++)
            {
_2320:          for (K=Z+1; K <= firstCard+4; K++)
                {
_2330:              if (_cardRanks[Z]<=_cardRanks[K]) { goto _2390; }
_2340:              X=_cards[Z];
_2350:              _cards[Z]=_cards[K];
_2360:              _cardRanks[Z]=_cardRanks[K];
_2370:              _cards[K]=X;
_2380:              _cardRanks[K]=GetRank(_cards[K]);
_2390:              ;
                }
            }
_2410:      X=0;
_2420:      for (Z=firstCard; Z <= firstCard+3; Z++)
            {
_2430:          if (_cardRanks[Z]!=_cardRanks[Z+1]) { goto _2470; }
_2440:          X += 11*(float)Math.Pow(10, Z-firstCard);
_2450:          D=_cards[Z];
_2460:          AnalyzeMultiples();
_2470:          ;
            }
_2480:      if (X!=0) { goto _2620; }
_2490:      if (_cardRanks[firstCard]+3!=_cardRanks[firstCard+3]) { goto _2520; }
_2500:      X=1111;
_2510:      U=10;
_2520:      if (_cardRanks[firstCard+1]+3!=_cardRanks[firstCard+4]) { goto _2620; }
_2530:      if (U!=10) { goto _2600; }
_2540:      U=14;
_2550:      HS="Straig";
_2560:      IS="ht";
_2570:      X=11111;
_2580:      D=_cards[firstCard+4];
_2590:      return;
_2600:      U=10;
_2610:      X=11110;
_2620:      if (U>=10) { goto _2690; }
_2630:      D=_cards[firstCard+4];
_2640:      HS="Schmal";
_2650:      IS="tz, ";
_2660:      U=9;
_2670:      X=11000;
_2680:      goto _2740;
_2690:      if (U!=10) { goto _2720; }
_2700:      if (I==1) { goto _2740; }
_2710:      goto _2750;
_2720:      if (U>12) { goto _2750; }
_2730:      if (GetRank(D)>6) { goto _2750; }
_2740:      I=6;
_2750:      return;
        }

        void AnalyzeMultiples()
        {
_2760:      if (U>=11) { goto _2810; }
_2770:      U=11;
_2780:      HS="A Pair";
_2790:      IS=" of ";
_2800:      return;
_2810:      if (U!=11) { goto _2910; }
_2820:      if (_cardRanks[Z]!=_cardRanks[Z-1]) { goto _2870; }
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
_2960:      if (_cardRanks[Z]!=_cardRanks[Z-1]) { goto _3010; }
_2970:      U=17;
_2980:      HS="Four";
_2990:      IS=" ";
_3000:      return;
_3010:      U=16;
_3020:      HS="Full H";
_3030:      IS="ouse, ";
_3040:      return;
        }

        bool GetWager()
        {
_3060:      _io.WriteLine();
            T = _io.ReadNumber("What is your bet");
_3080:      if ((T-(int)T)==0) { goto _3140; }
_3090:      if (K!=0) { goto _3120; }
_3100:      if (G!=0) { goto _3120; }
_3110:      if (T==.5) { return false; }
_3120:      _io.WriteLine("No small change, please.");
_3130:      goto _3060;
_3140:      if (_playerBalance-G-T>=0) { goto _3170; }
_3150:      if (PlayerCantRaiseFunds()) { return true; }
_3160:      goto _3060;
_3170:      if (T!=0) { goto _3200; }
_3180:      I=3;
_3190:      return Line_3380();
_3200:      if (G+T>=K) { goto _3230; }
_3210:      _io.WriteLine("If you can't see my bet, then fold.");
_3220:      goto _3060;
_3230:      G += T;
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
_3380:      _playerBalance -= G;
_3390:      _computerBalance -= K;
_3400:      _pot += G+K;
            return false;
        }

        bool Line_3420()
        {
_3420:      if (G>3*Z) { return Line_3350(); }
            return Line_3430();
        }

        bool Line_3430()
        {
_3430:      V=G-K+Get0To9();
_3440:      if (Line_3480()) { return true; }
_3450:      _io.WriteLine($"I'll see you, and raise you{V}");
_3460:      K=G+V;
_3470:      return GetWager();
        }

        bool Line_3480()
        {
_3480:      if (_computerBalance-G-V>=0) { goto _3660; }
_3490:      if (G!=0) { goto _3520; }
_3500:      V=_computerBalance;
_3510:      return false;
_3520:      if (_computerBalance-G>=0) { return Line_3360(); }
_3530:      if (O % 2 != 0) { goto _3600; }
_3540:      JS = _io.ReadString("Would you like to buy back your watch for $50");
_3560:      if (JS.StartsWith("N", InvariantCultureIgnoreCase)) { goto _3600; }
_3570:      _computerBalance=_computerBalance+50;
_3580:      O=O/2;
_3590:      return false;
_3600:      if (O % 3!= 0) { return CongratulatePlayer(); }
_3610:      JS = _io.ReadString("Would you like to buy back your tie tack for $50");
_3630:      if (JS.StartsWith("N", InvariantCultureIgnoreCase)) { return CongratulatePlayer(); }
_3640:      _computerBalance=_computerBalance+50;
_3650:      O=O/3;
_3660:      return false;
        }

        bool CongratulatePlayer()
        {
_3670:      _io.WriteLine("I'm busted.  Congratulations!");
_3680:      return true;  // STOP
        }

        void DisplayHandRank(string part1, string part2, int highCard)
        {
_3690:      _io.Write($"{part1}{part2}");
_3700:      if (part1!="A FLUS") { goto _3750; }
_3710:      ;
_3720:      _io.Write(GetSuitName(highCard/100));
_3730:      _io.WriteLine();
_3740:      return;
_3750:      _io.Write(GetRankName(GetRank(highCard)));
_3770:      if (part1=="Schmal") { goto _3790; }
_3780:      if (part1!="Straig") { goto _3810; }
_3790:      _io.WriteLine(" High");
_3800:      return;
_3810:      _io.WriteLine("'s");
_3820:      return;
        }

        bool PlayerCantRaiseFunds()
        {
_3830:      _io.WriteLine();
_3840:      _io.WriteLine("You can't bet with what you haven't got.");
_3850:      if (O % 2 == 0) { goto _3970; }
_3860:      _io.Write("Would you like to sell your watch");
_3870:      JS = _io.ReadString("");
_3880:      if (JS.StartsWith("N", InvariantCultureIgnoreCase)) { goto _3970; }
_3890:      if (Get0To9()>=7) { goto _3930; }
_3900:      _io.WriteLine("I'll give you $75 for it.");
_3910:      _playerBalance=_playerBalance+75;
_3920:      goto _3950;
_3930:      _io.WriteLine("That's a pretty crummy watch - I'll give you $25.");
_3940:      _playerBalance=_playerBalance+25;
_3950:      O=O*2;
_3960:      return false;
_3970:      if (O % 3 != 0) { goto _4090; }
_3980:      _io.WriteLine("Will you part with that diamond tie tack");
_3990:      JS = _io.ReadString("");
_4000:      if (JS.StartsWith("N", InvariantCultureIgnoreCase)) { goto _4080; }
_4010:      if (Get0To9()>=6) { goto _4050; }
_4020:      _io.WriteLine("You are now $100 richer.");
_4030:      _playerBalance=_playerBalance+100;
_4040:      goto _4070;
_4050:      _io.WriteLine("It's paste.  $25.");
_4060:      _playerBalance=_playerBalance+25;
_4070:      O=O*3;
_4080:      return false;
_4090:      _io.WriteLine("Your wad is shot.  So long, sucker!");
_4100:      return true;
        }
    }
}

internal static class IReadWriteExtensions
{
    internal static bool ReadYesNo(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var response = io.ReadString("Do you wish to continue");
            if (response.Equals("YES", InvariantCultureIgnoreCase)) { return true; }
            if (response.Equals("NO", InvariantCultureIgnoreCase)) { return false; }
            io.WriteLine("Answer Yes or No, please.");
        }
    }

    internal static int ReadNumber(this IReadWrite io, string prompt, int max, string maxPrompt)
    {
        io.Write(prompt);
        while (true)
        {
            var response = io.ReadNumber("");
            if (response <= max) { return (int)response; }
            io.WriteLine(maxPrompt);
        }
    }
}