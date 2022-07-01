using Poker.Cards;
using Poker.Players;
using Poker.Strategies;

namespace Poker;

internal class Table
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    public int Pot;

    public Table(IReadWrite io, IRandom random, Deck deck, Human human, Computer computer)
    {
        _io = io;
        _random = random;
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

    internal void PlayHand()
    {
        while (true)
        {
            _io.WriteLine();
            Computer.CheckFunds();
            if (Computer.IsBroke) { return; }

            _io.WriteLine($"The ante is ${Ante}.  I will deal:");
            _io.WriteLine();
            if (Human.Balance <= Ante)
            {
                Human.RaiseFunds();
                if (Human.IsBroke) { return; }
            }

            Deal(_random);

            _io.WriteLine();
            GetWagers("I'll open with ${0}", "I check.", allowRaiseAfterCheck: true);
            if (SomeoneIsBroke() || SomeoneHasFolded()) { return; }

            Draw();

            GetWagers();
            if (SomeoneIsBroke()) { return; }
            if (!Human.HasBet)
            {
                GetWagers("I'll bet ${0}", "I'll check");
            }
            if (SomeoneIsBroke() || SomeoneHasFolded()) { return; }
            if (GetWinner() is { } winner)
            {
                winner.TakeWinnings();
                return;
            }
        }
    }

    private void Deal(IRandom random)
    {
        Deck.Shuffle(random);

        Pot = Human.AnteUp() + Computer.AnteUp();

        Human.NewHand();
        Computer.NewHand();

        _io.WriteLine("Your hand:");
        _io.Write(Human.Hand);
    }

    private void Draw()
    {
        _io.WriteLine();
        _io.Write("Now we draw -- ");
        Human.DrawCards();
        Computer.DrawCards();
        _io.WriteLine();
    }

    private void GetWagers(string betFormat, string checkMessage, bool allowRaiseAfterCheck = false)
    {
        if (Computer.Strategy is Bet)
        {
            Computer.Bet = Computer.GetWager(Computer.Strategy.Value);
            if (Computer.IsBroke) { return; }

            _io.WriteLine(betFormat, Computer.Bet);
        }
        else
        {
            _io.WriteLine(checkMessage);
            if (!allowRaiseAfterCheck) { return; }
        }

        GetWagers();
    }

    private void GetWagers()
    {
        while (true)
        {
            Human.HasBet = false;
            while (true)
            {
                var humanStrategy = _io.ReadHumanStrategy(Computer.Bet == 0 && Human.Bet == 0);
                if (humanStrategy is Bet or Check)
                {
                    if (Human.Bet + humanStrategy.Value < Computer.Bet)
                    {
                        _io.WriteLine("If you can't see my bet, then fold.");
                        continue;
                    }
                    if (Human.Balance - Human.Bet - humanStrategy.Value >= 0)
                    {
                        Human.HasBet = true;
                        Human.Bet += humanStrategy.Value;
                        break;
                    }
                    Human.RaiseFunds();
                    if (Human.IsBroke) { return; }
                    continue;
                }
                else
                {
                    Human.Fold();
                    UpdatePot();
                    return;
                }
            }
            if (Human.Bet == Computer.Bet)
            {
                UpdatePot();
                return;
            }
            if (Computer.Strategy is Fold)
            {
                if (Human.Bet > 5)
                {
                    Computer.Fold();
                    _io.WriteLine("I fold.");
                    return;
                }
            }
            if (Human.Bet > 3 * Computer.Strategy.Value)
            {
                if (Computer.Strategy is not Raise)
                {
                    _io.WriteLine("I'll see you.");
                    Computer.Bet = Human.Bet;
                    UpdatePot();
                    return;
                }
            }

            var raise = Computer.GetWager(Human.Bet - Computer.Bet);
            if (Computer.IsBroke) { return; }
            _io.WriteLine($"I'll see you, and raise you {raise}");
            Computer.Bet = Human.Bet + raise;
        }
    }

    private void UpdatePot()
    {
        Human.Balance -= Human.Bet;
        Computer.Balance -= Computer.Bet;
        Pot += Human.Bet + Computer.Bet;
    }

    private bool SomeoneHasFolded()
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

    private bool SomeoneIsBroke() => Human.IsBroke || Computer.IsBroke;

    private Player? GetWinner()
    {
        _io.WriteLine();
        _io.WriteLine("Now we compare hands:");
        _io.WriteLine("My hand:");
        _io.Write(Computer.Hand);
        _io.WriteLine();
        _io.WriteLine($"You have {Human.Hand.Name}");
        _io.WriteLine($"and I have {Computer.Hand.Name}");
        if (Computer.Hand > Human.Hand) { return Computer; }
        if (Human.Hand > Computer.Hand) { return Human; }
        _io.WriteLine("The hand is drawn.");
        _io.WriteLine($"All $ {Pot} remains in the pot.");
        return null;
    }

    internal bool ShouldPlayAnotherHand()
    {
        if (Computer.IsBroke)
        {
            _io.WriteLine("I'm busted.  Congratulations!");
            return true;
        }

        if (Human.IsBroke)
        {
            _io.WriteLine("Your wad is shot.  So long, sucker!");
            return true;
        }

        _io.WriteLine($"Now I have $ {Computer.Balance} and you have $ {Human.Balance}");
        return _io.ReadYesNo("Do you wish to continue");
    }
}