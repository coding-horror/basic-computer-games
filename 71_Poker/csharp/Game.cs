using Poker.Cards;
using Poker.Players;
using Poker.Resources;

namespace Poker;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    private int Z;
    private int V;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    private int Get0To9() => _random.Next(10);

    internal void Play()
    {
        var deck = new Deck();
        var human = new Human(200, _io);
        var computer = new Computer(200, _io, _random);
        var table = new Table(_io, deck, human, computer);

        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);

        while (true)
        {
            var gameOver = PlayHand(table);
            if (gameOver) { break; }

            _io.WriteLine($"Now I have $ {table.Computer.Balance} and you have $ {table.Human.Balance}");
            if (!_io.ReadYesNo("Do you wish to continue")) { break; }
        }
    }

    internal bool PlayHand(Table table)
    {
        while(true)
        {
            _io.WriteLine();
            if (table.Computer.Balance <= table.Ante)
            {
                CongratulatePlayer();
                return false;
            }
            _io.WriteLine($"The ante is ${table.Ante}.  I will deal:");
            _io.WriteLine();
            if (table.Human.Balance <= table.Ante && table.Human.IsBroke()) { return false; }

            table.Deal();

            _io.WriteLine();
            Z = true switch
            {
                _ when table.Computer.Hand.IsWeak =>
                    table.Computer.BluffIf(Get0To9() < 2, 0b11100) ??
                    table.Computer.BluffIf(Get0To9() < 2, 0b11110) ??
                    table.Computer.BluffIf(Get0To9() < 1, 0b11111) ??
                    1,
                _ when table.Computer.Hand.Rank < HandRank.Three => table.Computer.BluffIf(Get0To9() < 2) ?? 0,
                _ when table.Computer.Hand.Rank < HandRank.FullHouse => 35,
                _ when Get0To9() < 1 => 35,
                _ => 2
            };
            if (Z <= 1)
            {
                _io.WriteLine("I check.");
            }
            else
            {
                V=Z+Get0To9();
                if (ComputerIsBroke()) { return false; }
                _io.WriteLine($"I'll open with ${V}");
                table.Computer.Bet = V;
            }
            if (GetWager()) { return false; }
            if (table.SomeoneHasFolded()) { return ShouldContinue(); }

            table.Draw();

            Z = true switch
            {
                _ when table.Computer.IsBluffing => 28,
                _ when table.Computer.Hand.IsWeak => 1,
                _ when table.Computer.Hand.Rank < HandRank.Three => Get0To9() == 0 ? 19 : 2,
                _ when table.Computer.Hand.Rank < HandRank.FullHouse => Get0To9() == 0 ? 11 : 19,
                _ => 2
            };

            if (GetWager()) { return false; }
            if (table.Human.HasBet)
            {
                if (table.SomeoneHasFolded()) { return ShouldContinue(); }
            }
            else if (!table.Computer.IsBluffing && table.Computer.Hand.IsWeak)
            {
                _io.WriteLine("I'll check");
            }
            else
            {
                V=Z+Get0To9();
                if (ComputerIsBroke()) { return false; }
                _io.WriteLine($"I'll bet ${V}");
                table.Computer.Bet = V;
                if (GetWager()) { return false; }
                if (table.SomeoneHasFolded()) { return ShouldContinue(); }
            }
            if (table.GetWinner() is { } winner)
            {
                winner.TakeWinnings();
                return ShouldContinue();
            }
        }

        bool ShouldContinue()
        {
            _io.WriteLine($"Now I have $ {table.Computer.Balance} and you have $ {table.Human.Balance} ");
            return _io.ReadYesNo("Do you wish to continue");
        }

        bool GetWager()
        {
            while (true)
            {
                table.Human.HasBet = false;
                while(true)
                {
                    if (_io.ReadPlayerAction(table.Computer.Bet == 0 && table.Human.Bet == 0) is Bet bet)
                    {
                        if (table.Human.Bet + bet.Amount < table.Computer.Bet)
                        {
                            _io.WriteLine("If you can't see my bet, then fold.");
                            continue;
                        }
                        if (table.Human.Balance - table.Human.Bet - bet.Amount >= 0)
                        {
                            table.Human.HasBet = true;
                            table.Human.Bet += bet.Amount;
                            break;
                        }
                        if (table.Human.IsBroke()) { return true; }
                        continue;
                    }
                    else
                    {
                        table.Human.Fold();
                        UpdatePot();
                        return false;
                    }
                }
                if (table.Human.Bet == table.Computer.Bet)
                {
                    UpdatePot();
                    return false;
                }
                if (Z == 1)
                {
                    if (table.Human.Bet > 5)
                    {
                        table.Computer.Fold();
                        _io.WriteLine("I fold.");
                        return false;
                    }
                    V = 5;
                }
                if (table.Human.Bet > 3 * Z)
                {
                    if (Z != 2)
                    {
                        _io.WriteLine("I'll see you.");
                        table.Computer.Bet = table.Human.Bet;
                        UpdatePot();
                        return false;
                    }
                }

                V = table.Human.Bet - table.Computer.Bet + Get0To9();
                if (ComputerIsBroke()) { return true; }
                _io.WriteLine($"I'll see you, and raise you{V}");
                table.Computer.Bet = table.Human.Bet + V;
            }
        }

        void UpdatePot()
        {
            table.Human.Balance -= table.Human.Bet;
            table.Computer.Balance -= table.Computer.Bet;
            table.Pot += table.Human.Bet + table.Computer.Bet;
        }

        bool ComputerIsBroke()
        {
            if (table.Computer.Balance - table.Human.Bet - V >= 0) { return false; }
            if (table.Human.Bet == 0)
            {
                V = table.Computer.Balance;
            }
            else if (table.Computer.Balance - table.Human.Bet >= 0)
            {
                _io.WriteLine("I'll see you.");
                table.Computer.Bet = table.Human.Bet;
                UpdatePot();
                return false;
            }
            else if (table.Computer.TrySellWatch())
            {
                return false;
            }
            CongratulatePlayer();
            return true;
        }

        void CongratulatePlayer() => _io.WriteLine("I'm busted.  Congratulations!");
    }
}

internal interface IAction { }
internal record Fold : IAction;
internal record Bet (int Amount) : IAction
{
    public Bet(float amount)
        : this((int)amount)
    {
    }
}
