using Poker.Cards;

namespace Poker.Players;

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

        if (Table.Computer.TryBuyWatch()) { return false; }

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
