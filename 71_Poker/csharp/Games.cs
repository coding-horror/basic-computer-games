using System.Text;
using Poker.Resources;
using static System.StringComparison;
using static Poker.Rank;

namespace Poker;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    private Hand _playerHand;
    private Hand _computerHand;

    private bool _hasWatch;
    private int _computerBalance;
    private int _playerBalance;
    private int _pot;

    private float _playerBet;
    private int _playerTotalBet;
    private int I;
    private int Z;
    private int _keepMask;
    private int _computerTotalBet;
    private int V;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    private int Get0To9() => _random.Next(10);

    internal void Play()
    {
        var deck = new Deck();

        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);

        _hasWatch = true;
        _computerBalance = 200;
        _playerBalance = 200;

        do
        {
            deck.Shuffle(_random);
        } while (PlayHand(deck));
    }

    internal bool PlayHand(Deck deck)
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
            _playerHand = deck.DealHand();
            _computerHand = deck.DealHand();

            _io.WriteLine("Your hand:");
            _io.Write(_playerHand);
            (_keepMask, I) = _computerHand.Analyze(2);
            _io.WriteLine();
_330:       if (I!=6) { goto _470; }
_340:       if (Get0To9()<=7) { goto _370; }
_350:       _keepMask = 0b11100;
_360:       goto _420;
_370:       if (Get0To9()<=7) { goto _400; }
_380:       _keepMask = 0b11110;
_390:       goto _420;
_400:       if (Get0To9()>=1) { goto _450; }
_410:       _keepMask = 0b11111;
_420:       I=7;
_430:       Z=23;
_440:       goto _580;
_450:       Z=1;
_460:       goto _510;
_470:       if (_computerHand.Rank >= 13) { goto _540; }
_480:       if (Get0To9()>=2) { goto _500; }
_490:       goto _420;
_500:       Z=0;
_510:       _computerTotalBet = 0;
_520:       _io.WriteLine("I check.");
_530:       goto _620;
_540:       Z = _computerHand.Rank <= 16 || Get0To9() < 1 ? 35 : 2;
_580:       V=Z+Get0To9();
_590:       if (ComputerCantContinue()) { return false; }
_600:       _io.WriteLine($"I'll open with ${V}");
_610:       _computerTotalBet = V;
            _playerTotalBet = 0;
_620:       if (GetWager()) { return false; }
_630:       var response = IsThereAWinner();
            if (response.HasValue) { return response.Value; }

            _io.WriteLine();
            var playerDrawCount = _io.ReadNumber(
                "Now we draw -- How many cards do you want",
                3,
                "You can't draw more than three cards.");
            if (playerDrawCount != 0)
            {
                Z=10;
                _io.WriteLine("What are their numbers:");
                for (var i = 1; i <= playerDrawCount; i++)
                {
                    _playerHand = _playerHand.Replace((int)_io.ReadNumber(""), deck.DealCard());
                }
                _io.WriteLine("Your new hand:");
                _io.Write(_playerHand);
            }
            var computerDrawCount = 0;
            for (var i = 1; i <= 5; i++)
            {
                if ((_keepMask & (1 << (i - 1))) == 0)
                {
                    _computerHand = _computerHand.Replace(i, deck.DealCard());
                    computerDrawCount++;
                }
            }
            _io.WriteLine();
            _io.Write($"I am taking {computerDrawCount} card");
            if (computerDrawCount != 1)
            {
                _io.WriteLine("s");
            }
            _io.WriteLine();
            V=I;
            (_keepMask, I) = _computerHand.Analyze(1);
            if (V == 7)
            {
                Z = 28;
            }
            else if (I == 6)
            {
                Z = 1;
            }
            else if (_computerHand.Rank < 13)
            {
                Z = Get0To9() == 6 ? 19 : 2;
            }
            else
            {
                if (_computerHand.Rank >= 16)
                {
                    Z = 2;
                }
                else
                {
                    Z = Get0To9() == 8 ? 11 : 19;
                }
            }
_1330:      _computerTotalBet = 0;
            _playerTotalBet = 0;
_1340:      if (GetWager()) { return false; }
_1350:      if (_playerBet!=.5) { goto _1450; }
_1360:      if (V==7) { goto _1400; }
_1370:      if (I!=6) { goto _1400; }
_1380:      _io.WriteLine("I'll check");
_1390:      goto _1460;
_1400:      V=Z+Get0To9();
_1410:      if (ComputerCantContinue()) { return false; }
_1420:      _io.WriteLine($"I'll bet ${V}");
_1430:      _computerTotalBet = V;
_1440:      if (GetWager()) { return false; }
_1450:      response = IsThereAWinner();
            if (response.HasValue) { return response.Value; }
_1460:      _io.WriteLine();
            _io.WriteLine("Now we compare hands:");
            _io.WriteLine("My hand:");
            _io.Write(_computerHand);
            _playerHand.Analyze(0);
            _io.WriteLine();
            _io.Write($"You have {_playerHand.Name}");
            _io.Write($"and I have {_computerHand.Name}");
            if (_computerHand > _playerHand) { return ComputerWins().Value; }
            if (_playerHand > _computerHand) { return PlayerWins().Value; }
            _io.WriteLine("The hand is drawn.");
            _io.WriteLine($"All ${_pot}remains in the pot.");
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

        bool GetWager()
        {
_3060:      _io.WriteLine();
            _playerBet = _io.ReadNumber("What is your bet");
_3080:      if ((_playerBet - (int)_playerBet) == 0) { goto _3140; }
_3090:      if (_computerTotalBet != 0) { goto _3120; }
_3100:      if (_playerTotalBet != 0) { goto _3120; }
_3110:      if (_playerBet == .5) { return false; }
_3120:      _io.WriteLine("No small change, please.");
_3130:      goto _3060;
_3140:      if (_playerBalance - _playerTotalBet - _playerBet>=0) { goto _3170; }
_3150:      if (PlayerCantRaiseFunds()) { return true; }
_3160:      goto _3060;
_3170:      if (_playerBet != 0) { goto _3200; }
_3180:      I=3;
_3190:      return Line_3380();
_3200:      if (_playerTotalBet + _playerBet >= _computerTotalBet) { goto _3230; }
_3210:      _io.WriteLine("If you can't see my bet, then fold.");
_3220:      goto _3060;
_3230:      _playerTotalBet += (int)_playerBet;
_3240:      if (_playerTotalBet == _computerTotalBet) { return Line_3380(); }
_3250:      if (Z!=1) { return Line_3420(); }
_3260:      if (_playerTotalBet>5) { goto _3300; }
_3270:      if (Z>=2) { return Line_3350(); }
_3280:      V=5;
_3290:      return Line_3420();
_3300:      if (Z==1) { goto _3320; }
_3310:      if (_playerBet <= 25) { return Line_3350(); }
_3320:      I=4;
_3330:      _io.WriteLine("I fold.");
_3340:      return false;
        }

        bool Line_3350()
        {
            if (Z==2) { return Line_3430(); }
            return Line_3360();
        }

        bool Line_3360()
        {
            _io.WriteLine("I'll see you.");
            _computerTotalBet = _playerTotalBet;
            return Line_3380();
        }

        bool Line_3380()
        {
            _playerBalance -= _playerTotalBet;
            _computerBalance -= _computerTotalBet;
            _pot += _playerTotalBet + _computerTotalBet;
            return false;
        }

        bool Line_3420()
        {
            if (_playerTotalBet>3*Z) { return Line_3350(); }
            return Line_3430();
        }

        bool Line_3430()
        {
            V = _playerTotalBet - _computerTotalBet + Get0To9();
            if (ComputerCantContinue()) { return true; }
            _io.WriteLine($"I'll see you, and raise you{V}");
            _computerTotalBet = _playerTotalBet + V;
            return GetWager();
        }

        bool ComputerCantContinue()
        {
            if (_computerBalance - _playerTotalBet - V >= 0) { return false; }
            if (_playerTotalBet == 0)
            {
                V = _computerBalance;
            }
            else if (_computerBalance - _playerTotalBet >= 0)
            {
                return Line_3360();
            }
            else if (!_hasWatch)
            {
                var response = _io.ReadString("Would you like to buy back your watch for $50");
                if (!response.StartsWith("N", InvariantCultureIgnoreCase))
                {
                    // The original code does not deduct $50 from the player
                    _computerBalance += 50;
                    _hasWatch = true;
                    return false;
                }
            }
            return CongratulatePlayer();
        }

        bool CongratulatePlayer()
        {
            _io.WriteLine("I'm busted.  Congratulations!");
            return true;
        }

        bool PlayerCantRaiseFunds()
        {
            _io.WriteLine();
            _io.WriteLine("You can't bet with what you haven't got.");

            if (_hasWatch)
            {
                var response = _io.ReadString("Would you like to sell your watch");
                if (!response.StartsWith("N", InvariantCultureIgnoreCase))
                {
                    if (Get0To9() < 7)
                    {
                        _io.WriteLine("I'll give you $75 for it.");
                        _playerBalance += 75;
                    }
                    else
                    {
                        _io.WriteLine("That's a pretty crummy watch - I'll give you $25.");
                        _playerBalance += 25;
                    }
                    _hasWatch = false;
                    return false;
                }
            }

            // The original program had some code about selling a tie tack, but due to a fault
            // in the logic the code was unreachable. I've omitted it in this port.

            _io.WriteLine("Your wad is shot.  So long, sucker!");
            return true;
        }
    }
}

internal class Hand
{
    private readonly Card[] _cards;
    private readonly Card _highCard;
    private readonly string _name1;
    private readonly string _name2;
    private readonly int _keepMask;
    private readonly Func<int, int> _iTransform;

    public Hand(IEnumerable<Card> cards)
    {
        _cards = cards.ToArray();
        (Rank, _name1, _name2, _highCard, _keepMask, _iTransform) = Analyze();
        Name = GetHandName();
    }

    public string Name { get; }
    public int Rank { get; }

    public Hand Replace(int cardNumber, Card newCard)
    {
        if (cardNumber < 1 || cardNumber > _cards.Length) { return this; }

        _cards[cardNumber - 1] = newCard;
        return new Hand(_cards);
    }

    public (int, int) Analyze(int i) => (_keepMask, _iTransform(i));

    private (int, string, string, Card, int, Func<int, int>) Analyze()
    {
        var suitMatchCount = 0;
        for (var i = 0; i < _cards.Length; i++)
        {
            if (i < _cards.Length-1 && _cards[i].Suit == _cards[i+1].Suit)
            {
                suitMatchCount++;
            }
        }
        if (suitMatchCount == 4)
        {
            return (15, "A Flus", "h in", _cards[0], 0b11111, x => x);
        }
        var sortedCards = _cards.OrderBy(c => c.Rank).ToArray();

        var handRank = 0;
        var keepMask = 0;
        Card highCard = default;
        var handName1 = "";
        var handName2 = "";
        for (var i = 0; i < sortedCards.Length - 1; i++)
        {
            if (sortedCards[i].Rank == sortedCards[i+1].Rank)
            {
                keepMask |= 0b11 << i;
                highCard = sortedCards[i];
                (handRank, handName1, handName2) =
                    (handRank, i > 0 && sortedCards[i].Rank == sortedCards[i - 1].Rank) switch
                    {
                        (<11, _) => (11, "A Pair", " of "),
                        (11, true) => (13, "Three", " "),
                        (11, _) => (12, "Two P", "air, "),
                        (12, _) => (16, "Full H", "ouse, "),
                        (_, true) => (17, "Four", " "),
                        _ => (16, "Full H", "ouse, ")
                    };
            }
        }
        if (keepMask == 0)
        {
            if (sortedCards[3] - sortedCards[0] == 3)
            {
                keepMask=0b1111;
                handRank=10;
            }
            if (sortedCards[4] - sortedCards[1] == 3)
            {
                if (handRank == 10)
                {
                    return (14, "Straig", "ht", sortedCards[4], 0b11111, x => x);
                }
                handRank=10;
                keepMask=0b11110;
            }
        }
        if (handRank < 10)
        {
            return (9, "Schmal", "tz, ", sortedCards[4], 0b11000, _ => 6);
        }
        var iTransform = Identity;
        if (handRank == 10)
        {
            iTransform = To6If1;
        }
        else if (handRank <= 12 && highCard.Rank <= 6)
        {
            iTransform = To6;
        }
        return (handRank, handName1, handName2, highCard, keepMask, iTransform);

        int Identity(int x) => x;
        int To6(int _) => 6;
        int To6If1(int x) => x == 1 ? 6 : x;
    }

    private string GetHandName()
    {
        var sb = new StringBuilder(_name1).Append(_name2);
        if (_name1 == "A Flus")
        {
            sb.Append(_highCard.Suit).AppendLine();
        }
        else
        {
            sb.Append(_highCard.Rank)
                .AppendLine(_name1 == "Schmal" || _name1 == "Straig" ? " High" : "'s");
        }
        return sb.ToString();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < _cards.Length; i++)
        {
            var cardDisplay = $" {i+1} --  {_cards[i]}";
            // Emulates the effect of the BASIC PRINT statement using the ',' to align text to 14-char print zones
            sb.Append(cardDisplay.PadRight(cardDisplay.Length + 14 - cardDisplay.Length % 14));
            if (i % 2 == 1)
            {
                sb.AppendLine();
            }
        }
        sb.AppendLine();
        return sb.ToString();
    }

    public static bool operator >(Hand x, Hand y) =>
        x.Rank > y.Rank ||
        x.Rank == y.Rank && x._highCard > y._highCard;

    public static bool operator <(Hand x, Hand y) =>
        x.Rank < y.Rank ||
        x.Rank == y.Rank && x._highCard < y._highCard;
}

internal enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

internal class Deck
{
    private readonly Card[] _cards;
    private int _nextCard;

    public Deck()
    {
        _cards = Ranks.SelectMany(r => Enum.GetValues<Suit>().Select(s => new Card(r, s))).ToArray();
    }

    public void Shuffle(IRandom _random)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            var j = _random.Next(_cards.Length);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }

    public Card DealCard() => _cards[_nextCard++];

    public Hand DealHand() => new Hand(Enumerable.Range(0, 5).Select(_ => DealCard()));
}

internal record struct Card (Rank Rank, Suit Suit)
{
    public override string ToString() => $"{Rank} of {Suit}";

    public static bool operator <(Card x, Card y) => x.Rank < y.Rank;
    public static bool operator >(Card x, Card y) => x.Rank > y.Rank;

    public static int operator -(Card x, Card y) => x.Rank - y.Rank;
}

internal struct Rank : IComparable<Rank>
{
    public static IEnumerable<Rank> Ranks => new[]
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    };

    public static Rank Two = new(2);
    public static Rank Three = new(3);
    public static Rank Four = new(4);
    public static Rank Five = new(5);
    public static Rank Six = new(6);
    public static Rank Seven = new(7);
    public static Rank Eight = new(8);
    public static Rank Nine = new(9);
    public static Rank Ten = new(10);
    public static Rank Jack = new(11, "Jack");
    public static Rank Queen = new(12, "Queen");
    public static Rank King = new(13, "King");
    public static Rank Ace = new(14, "Ace");

    private readonly int _value;
    private readonly string _name;

    private Rank(int value, string? name = null)
    {
        _value = value;
        _name = name ?? $" {value} ";
    }

    public override string ToString() => _name;

    public int CompareTo(Rank other) => this - other;

    public static bool operator <(Rank x, Rank y) => x._value < y._value;
    public static bool operator >(Rank x, Rank y) => x._value > y._value;
    public static bool operator ==(Rank x, Rank y) => x._value == y._value;
    public static bool operator !=(Rank x, Rank y) => x._value != y._value;

    public static int operator -(Rank x, Rank y) => x._value - y._value;

    public static bool operator <=(Rank rank, int value) => rank._value <= value;
    public static bool operator >=(Rank rank, int value) => rank._value >= value;
}

internal static class IReadWriteExtensions
{
    internal static bool ReadYesNo(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var response = io.ReadString(prompt);
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