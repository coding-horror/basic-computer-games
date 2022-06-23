using Poker.Cards;
using Poker.Players;

namespace Poker;

internal class Table
{
    private readonly IReadWrite _io;
    public int Pot;

    public Table(IReadWrite io, Deck deck, Human human, Computer computer)
    {
        _io = io;
        Deck = deck;
        Human = human;
        Computer = computer;

        human.Sit(this);
        computer.Sit(this);
    }

    public int Ante { get; } = 5;
    public Deck Deck { get; }
    public Human Human { get; }
    public Computer Computer { get; }

    public void Deal(IRandom random)
    {
        Deck.Shuffle(random);

        Pot = Human.AnteUp() + Computer.AnteUp();

        Human.NewHand();
        Computer.NewHand();

        _io.WriteLine("Your hand:");
        _io.Write(Human.Hand);
    }

    public void Draw()
    {
        _io.WriteLine();
        _io.Write("Now we draw -- ");
        Human.DrawCards();
        Computer.DrawCards();
        _io.WriteLine();
    }

    public void AcceptBets()
    {

    }

    public void UpdatePot()
    {
        Human.Balance -= Human.Bet;
        Computer.Balance -= Computer.Bet;
        Pot += Human.Bet + Computer.Bet;
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