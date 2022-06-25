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
    public bool HasBet { get; set; }
    public int Bet { get; set; }
    public bool HasFolded => _hasFolded;
    public bool IsBroke { get; protected set; }

    protected Table Table =>
        _table ?? throw new InvalidOperationException("The player must be sitting at the table.");

    public void Sit(Table table) => _table = table;

    public virtual void NewHand()
    {
        Bet = 0;
        Hand = Table.Deck.DealHand();
        _hasFolded = false;
    }

    public int AnteUp()
    {
        Balance -= Table.Ante;
        return Table.Ante;
    }

    public void DrawCards()
    {
        Bet = 0;
        DrawCards(Table.Deck);
    }

    protected abstract void DrawCards(Deck deck);

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
