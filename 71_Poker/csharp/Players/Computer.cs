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
        Strategy = Strategy.None;
    }

    public Strategy Strategy { get; set; }

    public override void NewHand()
    {
        base.NewHand();

        Strategy = (Hand.IsWeak, Hand.Rank < HandRank.Three, Hand.Rank < HandRank.FullHouse) switch
        {
            (true, _, _) when _random.Next(10) < 2 => Strategy.Bluff(23, 0b11100),
            (true, _, _) when _random.Next(10) < 2 => Strategy.Bluff(23, 0b11110),
            (true, _, _) when _random.Next(10) < 1 => Strategy.Bluff(23, 0b11111),
            (true, _, _) => Strategy.Fold,
            (false, true, _) => _random.Next(10) < 2 ? Strategy.Bluff(23) : Strategy.Check,
            (false, false, true) => Strategy.Bet(35),
            (false, false, false) => _random.Next(10) < 1 ? Strategy.Bet(35) : Strategy.Raise
        };
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

        Strategy = (Hand.IsWeak, Hand.Rank < HandRank.Three, Hand.Rank < HandRank.FullHouse) switch
        {
            _ when Strategy is Bluff => Strategy.Bluff(28),
            (true, _, _) => Strategy.Fold,
            (false, true, _) => _random.Next(10) == 0 ? Strategy.Bet(19) : Strategy.Raise,
            (false, false, true) => _random.Next(10) == 0 ? Strategy.Bet(11) : Strategy.Bet(19),
            (false, false, false) => Strategy.Raise
        };
    }

    public int GetWager(int wager)
    {
        wager += _random.Next(10);
        if (Balance < Table.Human.Bet + wager)
        {
            if (Table.Human.Bet == 0) { return Balance; }

            if (Balance >= Table.Human.Bet)
            {
                _io.WriteLine("I'll see you.");
                Bet = Table.Human.Bet;
                Table.CollectBets();
            }
            else
            {
                RaiseFunds();
            }
        }

        return wager;
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

    public void RaiseFunds()
    {
        if (Table.Human.HasWatch) { return; }

        var response = _io.ReadString("Would you like to buy back your watch for $50");
        if (response.StartsWith("N", InvariantCultureIgnoreCase)) { return; }

        // The original code does not deduct $50 from the player
        Balance += 50;
        Table.Human.ReceiveWatch();
        IsBroke = true;
    }

    public void CheckFunds() { IsBroke = Balance <= Table.Ante; }

    public override void TakeWinnings()
    {
        _io.WriteLine("I win.");
        base.TakeWinnings();
    }
}
