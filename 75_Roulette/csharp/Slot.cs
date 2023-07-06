using System.Collections.Immutable;

namespace Roulette;

internal class Slot
{
    private readonly ImmutableHashSet<BetType> _coveringBets;

    public Slot (string name, params BetType[] coveringBets)
    {
        Name = name;
        _coveringBets = coveringBets.ToImmutableHashSet();
    }

    public string Name { get; }

    public bool IsCoveredBy(Bet bet) => _coveringBets.Contains(bet.Type);
}
