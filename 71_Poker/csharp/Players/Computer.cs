using Poker.Cards;
using Poker.Strategies;
using static System.StringComparison;

namespace Poker.Players;

internal class Computer : Player
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Computer(int bank, IReadWrite io, IRandom random)
        : base(bank)
    {
        _io = io;
        _random = random;
        Strategy = Strategy.Check;
    }

    public Strategy Strategy { get; set; }

    public override void NewHand()
    {
        base.NewHand();
    }

    protected override void DrawCards(Deck deck)
    {
        var keepMask = Strategy.KeepMask ?? Hand.KeepMask;
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

    public bool TryBuyWatch()
    {
        if (!Table.Human.HasWatch) { return false; }

        var response = _io.ReadString("Would you like to sell your watch");
        if (response.StartsWith("N", InvariantCultureIgnoreCase)) { return false; }

        var (value, message) = (_random.Next(10) < 7) switch
        {
            true => (75, "I'll give you $75 for it."),
            false => (25, "That's a pretty crummy watch - I'll give you $25.")
        };

        _io.WriteLine(message);
        Table.Human.SellWatch(value);
        // The original code does not have the computer part with any money

        return true;
    }

    public bool TrySellWatch()
    {
        if (Table.Human.HasWatch) { return false; }

        var response = _io.ReadString("Would you like to buy back your watch for $50");
        if (response.StartsWith("N", InvariantCultureIgnoreCase)) { return false; }

        // The original code does not deduct $50 from the player
        Balance += 50;
        Table.Human.ReceiveWatch();
        return true;
    }

    public override void TakeWinnings()
    {
        _io.WriteLine("I win.");
        base.TakeWinnings();
    }
}
