using Poker.Cards;
using static System.StringComparison;

namespace Poker.Players;

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
