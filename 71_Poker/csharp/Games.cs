using Poker.Resources;
using static System.StringComparison;

namespace Poker;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;


    private int _playerBet;
    private int _playerTotalBet;
    private int I;
    private int Z;
    private int _computerTotalBet;
    private int V;
    private bool _playerFolds;
    private bool _computerFolds;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    private int Get0To9() => _random.Next(10);

    internal void Play()
    {
        var deck = new Deck();
        var human = new Human(200);
        var computer = new Computer(200);
        var table = new Table(_io, deck, human, computer);

        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);

        do
        {
            deck.Shuffle(_random);
        } while (PlayHand(table));
    }

    internal bool PlayHand(Table table)
    {
        _playerFolds = _computerFolds = false;
        table.Pot=0;
        while(true)
        {
            _io.WriteLine();
            if (table.Computer.Balance<=5)
            {
                CongratulatePlayer();
                return false;
            }
            _io.WriteLine("The ante is $5.  I will deal:");
            _io.WriteLine();
            if (table.Human.Balance <= 5 && PlayerIsBroke()) { return false; }

            table.Deal();

            (table.Computer.KeepMask, I) = table.Computer.Hand.Analyze(2);
            _io.WriteLine();
            if (I == 6)
            {
                Z=1;
                if (Get0To9() > 7)
                {
                    table.Computer.KeepMask = 0b11100;
                    I=7;
                    Z=23;
                }
                else if (Get0To9() > 7)
                {
                    table.Computer.KeepMask = 0b11110;
                    I=7;
                    Z=23;
                }
                else if (Get0To9() < 1)
                {
                    table.Computer.KeepMask = 0b11111;
                    I=7;
                    Z=23;
                }
            }
            else
            {
                Z=0;
                if (table.Computer.Hand.Rank >= 13)
                {
                    Z = table.Computer.Hand.Rank <= 16 || Get0To9() < 1 ? 35 : 2;
                }
                else if (Get0To9() < 2)
                {
                    I=7;
                    Z=23;
                }
            }
            if (Z <= 1)
            {
                _computerTotalBet = 0;
                _io.WriteLine("I check.");
            }
            else
            {
                V=Z+Get0To9();
                if (ComputerCantContinue()) { return false; }
                _io.WriteLine($"I'll open with ${V}");
                _computerTotalBet = V;
                _playerTotalBet = 0;
            }
            if (GetWager()) { return false; }
            if (CheckIfSomeoneFolded() is {} response) { return response; }

            table.Draw();

            V=I;
            (_, I) = table.Computer.Hand.Analyze(1);
            if (V == 7)
            {
                Z = 28;
            }
            else if (I == 6)
            {
                Z = 1;
            }
            else if (table.Computer.Hand.Rank < 13)
            {
                Z = Get0To9() == 6 ? 19 : 2;
            }
            else
            {
                if (table.Computer.Hand.Rank >= 16)
                {
                    Z = 2;
                }
                else
                {
                    Z = Get0To9() == 8 ? 11 : 19;
                }
            }
            _computerTotalBet = 0;
            _playerTotalBet = 0;
            if (GetWager()) { return false; }
            if (_playerBet != 0)
            {
                if (CheckIfSomeoneFolded() is {} response2) { return response2; }
            }
            else if (V != 7 && I == 6)
            {
                _io.WriteLine("I'll check");
            }
            else
            {
                V=Z+Get0To9();
                if (ComputerCantContinue()) { return false; }
                _io.WriteLine($"I'll bet ${V}");
                _computerTotalBet = V;
                if (GetWager()) { return false; }
                if (CheckIfSomeoneFolded() is {} response3) { return response3; }
            }
            _io.WriteLine();
            _io.WriteLine("Now we compare hands:");
            _io.WriteLine("My hand:");
            _io.Write(table.Computer.Hand);
            table.Human.Hand.Analyze(0);
            _io.WriteLine();
            _io.Write($"You have {table.Human.Hand.Name}");
            _io.Write($"and I have {table.Computer.Hand.Name}");
            if (table.Computer.Hand > table.Human.Hand) { return ComputerWins(); }
            if (table.Human.Hand > table.Computer.Hand) { return PlayerWins(); }
            _io.WriteLine("The hand is drawn.");
            _io.WriteLine($"All $ {table.Pot} remains in the pot.");
        }

        bool? CheckIfSomeoneFolded()
        {
            if (_playerFolds)
            {
                _io.WriteLine();
                return ComputerWins();
            }
            else if (_computerFolds)
            {
                _io.WriteLine();
                return PlayerWins();
            }
            else
            {
                return null;
            }
        }

        bool ComputerWins()
        {
            _io.WriteLine("I win.");
            table.Computer.Balance += table.Pot;
            return ShouldContinue();
        }

        bool ShouldContinue()
        {
            _io.WriteLine($"Now I have ${table.Computer.Balance}and you have ${table.Human.Balance}");
            return _io.ReadYesNo("Do you wish to continue");
        }

        bool PlayerWins()
        {
            _io.WriteLine("You win.");
            table.Human.Balance += table.Pot;
            return ShouldContinue();
        }

        bool GetWager()
        {
            _playerBet = 0;
            while(true)
            {
                if (_io.ReadPlayerAction(_computerTotalBet == 0 && _playerTotalBet == 0) is Bet bet)
                {
                    if (_playerTotalBet + bet.Amount < _computerTotalBet)
                    {
                        _io.WriteLine("If you can't see my bet, then fold.");
                        continue;
                    }
                    if (table.Human.Balance - _playerTotalBet - bet.Amount >= 0)
                    {
                        _playerBet = bet.Amount;
                        _playerTotalBet += bet.Amount;
                        break;
                    }
                    if (PlayerIsBroke()) { return true; }
                    continue;
                }
                else
                {
                    _playerFolds = true;
                    return UpdatePot();
                }
            }
            if (_playerTotalBet == _computerTotalBet) { return UpdatePot(); }
            if (Z == 1)
            {
                if (_playerTotalBet > 5)
                {
                    _computerFolds = true;
                    _io.WriteLine("I fold.");
                    return false;
                }
                V = 5;
            }
            return Line_3420();
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
            return UpdatePot();
        }

        bool UpdatePot()
        {
            table.Human.Balance -= _playerTotalBet;
            table.Computer.Balance -= _computerTotalBet;
            table.Pot += _playerTotalBet + _computerTotalBet;
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
            if (table.Computer.Balance - _playerTotalBet - V >= 0) { return false; }
            if (_playerTotalBet == 0)
            {
                V = table.Computer.Balance;
            }
            else if (table.Computer.Balance - _playerTotalBet >= 0)
            {
                return Line_3360();
            }
            else if (table.Computer.TrySellWatch(table.Human, _io))
            {
                return false;
            }
            return CongratulatePlayer();
        }

        bool CongratulatePlayer()
        {
            _io.WriteLine("I'm busted.  Congratulations!");
            return true;
        }

        bool PlayerIsBroke()
        {
            _io.WriteLine();
            _io.WriteLine("You can't bet with what you haven't got.");

            if (Computer.TryBuyWatch(table.Human, _io, _random)) { return false; }

            // The original program had some code about selling a tie tack, but due to a fault
            // in the logic the code was unreachable. I've omitted it in this port.

            _io.WriteLine("Your wad is shot.  So long, sucker!");
            return true;
        }
    }
}

internal interface IAction { }
internal record Fold : IAction;
internal record Bet (int Amount) : IAction
{
    public Bet(float amount)
        : this((int)amount)
    {
    }
}

internal abstract class Player
{
    protected Player(int bank)
    {
        Hand = Hand.Empty;
        Balance = bank;
    }

    public Hand Hand { get; set; }
    public int Balance { get; set; }
    public int Bet { get; private set; }

    public void Pay(int amount)
    {
        Balance -= amount;
    }
}

internal class Human : Player
{
    public Human(int bank)
        : base(bank)
    {
        HasWatch = true;
    }

    public bool HasWatch { get; set; }

    public void DrawCards(Deck deck, IReadWrite io)
    {
        var count = io.ReadNumber("How many cards do you want", 3, "You can't draw more than three cards.");
        if (count == 0) { return; }

        io.WriteLine("What are their numbers:");
        for (var i = 1; i <= count; i++)
        {
            Hand = Hand.Replace((int)io.ReadNumber(), deck.DealCard());
        }

        io.WriteLine("Your new hand:");
        io.Write(Hand);
    }

    public void ReceiveWatch()
    {
        // In the original code the player does not pay any money to receive the watch back.
        HasWatch = true;
    }

    public void SellWatch(int amount)
    {
        HasWatch = false;
        Balance += amount;
    }
}

internal class Computer : Player
{
    public Computer(int bank)
        : base(bank)
    {
    }

    public int KeepMask { get; set; }

    public void DrawCards(Deck deck, IReadWrite io)
    {
        var count = 0;
        for (var i = 1; i <= 5; i++)
        {
            if ((KeepMask & (1 << (i - 1))) == 0)
            {
                Hand = Hand.Replace(i, deck.DealCard());
                count++;
            }
        }

        io.WriteLine();
        io.Write($"I am taking {count} card");
        if (count != 1)
        {
            io.WriteLine("s");
        }
    }

    public static bool TryBuyWatch(Human human, IReadWrite io, IRandom random)
    {
        if (!human.HasWatch) { return false; }

        var response = io.ReadString("Would you like to sell your watch");
        if (response.StartsWith("N", InvariantCultureIgnoreCase)) { return false; }

        var (value, message) = (random.Next(10) < 7) switch
        {
            true => (75, "I'll give you $75 for it."),
            false => (25, "That's a pretty crummy watch - I'll give you $25.")
        };

        io.WriteLine(message);
        human.SellWatch(value);

        return true;
    }

    public bool TrySellWatch(Human human, IReadWrite io)
    {
        if (human.HasWatch) { return false; }

        var response = io.ReadString("Would you like to buy back your watch for $50");
        if (response.StartsWith("N", InvariantCultureIgnoreCase)) { return false; }

        // The original code does not deduct $50 from the player
        Balance += 50;
        human.ReceiveWatch();
        return true;
    }
}

internal class Table
{
    private IReadWrite _io;
    public int Pot;
    private Deck _deck;

    public Table(IReadWrite io, Deck deck, Human human, Computer computer)
    {
        _io = io;
        _deck = deck;
        Human = human;
        Computer = computer;
    }

    public Human Human { get; }
    public Computer Computer { get; }

    public void Deal()
    {
        Pot += 10;
        Human.Pay(5);
        Computer.Pay(5);

        Human.Hand = _deck.DealHand();
        Computer.Hand = _deck.DealHand();

        _io.WriteLine("Your hand:");
        _io.Write(Human.Hand);
    }

    public void Draw()
    {
        _io.WriteLine();
        _io.Write("Now we draw -- ");
        Human.DrawCards(_deck, _io);
        Computer.DrawCards(_deck, _io);
        _io.WriteLine();
    }

    public void AcceptBets()
    {

    }
}