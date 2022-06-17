using Poker.Resources;
using static System.StringComparison;

namespace Poker;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    private int _playerBet;
    private int _playerTotalBet;
    private int Z;
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
        var human = new Human(200, _io);
        var computer = new Computer(200, _io, _random);
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
            if (table.Human.Balance <= 5 && table.Human.IsBroke()) { return false; }

            table.Deal();

            _io.WriteLine();
            Z = table.Computer.Hand.Rank switch
            {
                _ when table.Computer.Hand.IsWeak =>
                    table.Computer.BluffIf(Get0To9() > 7, 0b11100) ??
                    table.Computer.BluffIf(Get0To9() > 7, 0b11110) ??
                    table.Computer.BluffIf(Get0To9() < 1, 0b11111) ??
                    1,
                < 13 => table.Computer.BluffIf(Get0To9() < 2) ?? 0,
                <= 16 => 35,
                _ when Get0To9() < 1 => 35,
                _ => 2
            };
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
            if (table.SomeoneHasFolded()) { return ShouldContinue(); }

            table.Draw();

            Z = table.Computer.Hand.Rank switch
            {
                _ when table.Computer.IsBluffing => 28,
                _ when table.Computer.Hand.IsWeak => 1,
                < 13 => Get0To9() == 0 ? 19 : 2,
                < 16 => Get0To9() == 8 ? 11 : 19,
                _ => 2
            };

            _computerTotalBet = 0;
            _playerTotalBet = 0;
            if (GetWager()) { return false; }
            if (_playerBet != 0)
            {
                if (table.SomeoneHasFolded()) { return ShouldContinue(); }
            }
            else if (!table.Computer.IsBluffing && table.Computer.Hand.IsWeak)
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
                if (table.SomeoneHasFolded()) { return ShouldContinue(); }
            }
            if (table.GetWinner() is { } winner)
            {
                winner.TakeWinnings();
                return ShouldContinue();
            }
        }

        bool ShouldContinue()
        {
            _io.WriteLine($"Now I have $ {table.Computer.Balance} and you have $ {table.Human.Balance} ");
            return _io.ReadYesNo("Do you wish to continue");
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
                    if (table.Human.IsBroke()) { return true; }
                    continue;
                }
                else
                {
                    table.Human.Fold();
                    return UpdatePot();
                }
            }
            if (_playerTotalBet == _computerTotalBet) { return UpdatePot(); }
            if (Z == 1)
            {
                if (_playerTotalBet > 5)
                {
                    table.Computer.Fold();
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
            else if (table.Computer.TrySellWatch(table.Human))
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
    private Table? _table;
    private bool _hasFolded;

    protected Player(int bank)
    {
        Hand = Hand.Empty;
        Balance = bank;
    }

    public Hand Hand { get; set; }
    public int Balance { get; set; }
    public int Bet { get; private set; }
    public bool HasFolded => _hasFolded;

    protected Table Table =>
        _table ?? throw new InvalidOperationException("The player must be sitting at the table.");

    public void Sit(Table table) => _table = table;

    public virtual void NewHand(Hand hand)
    {
        Hand = hand;
        _hasFolded = false;
    }

    public void Pay(int amount)
    {
        Balance -= amount;
    }

    public virtual void TakeWinnings()
    {
        Balance += Table.Pot;
        Table.Pot = 0;
    }

    public void Fold()
    {
        _hasFolded = true;
    }
}

internal class Human : Player
{
    private readonly IReadWrite _io;

    public Human(int bank, IReadWrite io)
        : base(bank)
    {
        HasWatch = true;
        _io = io;
    }

    public bool HasWatch { get; set; }

    public void DrawCards(Deck deck)
    {
        var count = _io.ReadNumber("How many cards do you want", 3, "You can't draw more than three cards.");
        if (count == 0) { return; }

        _io.WriteLine("What are their numbers:");
        for (var i = 1; i <= count; i++)
        {
            Hand = Hand.Replace((int)_io.ReadNumber(), deck.DealCard());
        }

        _io.WriteLine("Your new hand:");
        _io.Write(Hand);
    }

    public bool IsBroke()
    {
        _io.WriteLine();
        _io.WriteLine("You can't bet with what you haven't got.");

        if (Table.Computer.TryBuyWatch(this)) { return false; }

        // The original program had some code about selling a tie tack, but due to a fault
        // in the logic the code was unreachable. I've omitted it in this port.

        _io.WriteLine("Your wad is shot.  So long, sucker!");
        return true;
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

    public override void TakeWinnings()
    {
        _io.WriteLine("You win.");
        base.TakeWinnings();
    }
}

internal class Computer : Player
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private bool _isBluffing;

    public Computer(int bank, IReadWrite io, IRandom random)
        : base(bank)
    {
        _io = io;
        _random = random;
    }

    public bool IsBluffing => _isBluffing;

    public override void NewHand(Hand hand)
    {
        base.NewHand(hand);
        _isBluffing = false;
    }

    public int? BluffIf(bool shouldBluff, int? keepMask = null)
    {
        if (!shouldBluff) { return null; }

        _isBluffing = true;
        Hand.KeepMask = keepMask ?? Hand.KeepMask;
        return 23;
    }

    public void DrawCards(Deck deck)
    {
        var keepMask = Hand.KeepMask;
        var count = 0;
        for (var i = 1; i <= 5; i++)
        {
            if ((keepMask & (1 << (i - 1))) == 0)
            {
                Hand = Hand.Replace(i, deck.DealCard());
                count++;
            }
        }

        _io.WriteLine();
        _io.Write($"I am taking {count} card");
        if (count != 1)
        {
            _io.WriteLine("s");
        }
    }

    public bool TryBuyWatch(Human human)
    {
        if (!human.HasWatch) { return false; }

        var response = _io.ReadString("Would you like to sell your watch");
        if (response.StartsWith("N", InvariantCultureIgnoreCase)) { return false; }

        var (value, message) = (_random.Next(10) < 7) switch
        {
            true => (75, "I'll give you $75 for it."),
            false => (25, "That's a pretty crummy watch - I'll give you $25.")
        };

        _io.WriteLine(message);
        human.SellWatch(value);
        // The original code does not have the computer part with any money

        return true;
    }

    public bool TrySellWatch(Human human)
    {
        if (human.HasWatch) { return false; }

        var response = _io.ReadString("Would you like to buy back your watch for $50");
        if (response.StartsWith("N", InvariantCultureIgnoreCase)) { return false; }

        // The original code does not deduct $50 from the player
        Balance += 50;
        human.ReceiveWatch();
        return true;
    }

    public override void TakeWinnings()
    {
        _io.WriteLine("I win.");
        base.TakeWinnings();
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

        human.Sit(this);
        computer.Sit(this);
    }

    public Human Human { get; }
    public Computer Computer { get; }

    public void Deal()
    {
        Pot = 10;
        Human.Pay(5);
        Computer.Pay(5);

        Human.NewHand(_deck.DealHand());
        Computer.NewHand(_deck.DealHand());

        _io.WriteLine("Your hand:");
        _io.Write(Human.Hand);
    }

    public void Draw()
    {
        _io.WriteLine();
        _io.Write("Now we draw -- ");
        Human.DrawCards(_deck);
        Computer.DrawCards(_deck);
        _io.WriteLine();
    }

    public void AcceptBets()
    {

    }

    public bool SomeoneHasFolded()
    {
        if (Human.HasFolded)
        {
            _io.WriteLine();
            Computer.TakeWinnings();
        }
        else if (Computer.HasFolded)
        {
            _io.WriteLine();
            Human.TakeWinnings();
        }
        else
        {
            return false;
        }

        Pot = 0;
        return true;
    }

    public Player? GetWinner()
    {
        _io.WriteLine();
        _io.WriteLine("Now we compare hands:");
        _io.WriteLine("My hand:");
        _io.Write(Computer.Hand);
        _io.WriteLine();
        _io.Write($"You have {Human.Hand.Name}");
        _io.Write($"and I have {Computer.Hand.Name}");
        if (Computer.Hand > Human.Hand) { return Computer; }
        if (Human.Hand > Computer.Hand) { return Human; }
        _io.WriteLine("The hand is drawn.");
        _io.WriteLine($"All $ {Pot} remains in the pot.");
        return null;
    }
}