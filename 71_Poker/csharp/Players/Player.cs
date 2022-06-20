using Poker.Cards;

namespace Poker.Players;

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

    public int AnteUp()
    {
        Balance -= Table.Ante;
        return Table.Ante;
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
