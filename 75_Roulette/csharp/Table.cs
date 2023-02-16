namespace Roulette;

internal class Table
{
    private readonly IReadWrite _io;
    private readonly Wheel _wheel;
    private readonly Croupier _croupier;

    public Table(Croupier croupier, IReadWrite io, IRandom random)
    {
        _croupier = croupier;
        _io = io;
        _wheel = new(random);
    }

    public bool Play()
    {
        var bets = AcceptBets();
        var slot = SpinWheel();
        SettleBets(bets, slot);

        _io.Write(_croupier.Totals);

        if (_croupier.PlayerIsBroke || _croupier.HouseIsBroke) { return false; }

        return _io.ReadString(Prompts.Again).ToLowerInvariant().StartsWith('y');
    }

    private Slot SpinWheel()
    {
        _io.Write(Streams.Spinning);
        var slot = _wheel.Spin();
        _io.Write(slot.Name);
        return slot;
    }

    private IReadOnlyList<Bet> AcceptBets()
    {
        var betCount = _io.ReadBetCount();
        var betTypes = new HashSet<BetType>();
        var bets = new List<Bet>();
        for (int i = 1; i <= betCount; i++)
        {
            while (!TryAdd(_io.ReadBet(i)))
            {
                _io.Write(Streams.BetAlready);
            }
        }

        return bets.AsReadOnly();

        bool TryAdd(Bet bet)
        {
            if (betTypes.Add(bet.Type))
            {
                bets.Add(bet);
                return true;
            }

            return false;
        }
    }

    private void SettleBets(IReadOnlyList<Bet> bets, Slot slot)
    {
        foreach (var bet in bets)
        {
            _io.Write(slot.IsCoveredBy(bet) ? _croupier.Pay(bet) : _croupier.Take(bet));
        }
    }
}
