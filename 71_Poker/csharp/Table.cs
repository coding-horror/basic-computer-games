using Poker.Cards;
using Poker.Players;

namespace Poker;

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