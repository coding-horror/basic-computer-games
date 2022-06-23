using Poker.Cards;
using Poker.Players;
using Poker.Resources;
using Poker.Strategies;

namespace Poker;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

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
        while (true)
        {
            _io.WriteLine();
            if (table.Computer.Balance <= table.Ante)
            {
                _io.WriteLine("I'm busted.  Congratulations!");
                return true;
            }
            _io.WriteLine($"The ante is ${table.Ante}.  I will deal:");
            _io.WriteLine();
            if (table.Human.Balance <= table.Ante && table.Human.IsBroke()) { return true; }

            table.Deal(_random);

            _io.WriteLine();
            table.Computer.Strategy = (table.Computer.Hand.IsWeak, table.Computer.Hand.Rank < HandRank.Three, table.Computer.Hand.Rank < HandRank.FullHouse) switch
            {
                (true, _, _) when Get0To9() < 2 => Strategy.Bluff(23, 0b11100),
                (true, _, _) when Get0To9() < 2 => Strategy.Bluff(23, 0b11110),
                (true, _, _) when Get0To9() < 1 => Strategy.Bluff(23, 0b11111),
                (true, _, _) =>  Strategy.Fold,
                (false, true, _) => Get0To9() < 2 ? Strategy.Bluff(23) : Strategy.Check,
                (false, false, true) => Strategy.Bet(35),
                (false, false, false) => Get0To9() < 1 ? Strategy.Bet(35) : Strategy.Raise
            };
            if (table.Computer.Strategy is Strategies.Bet)
            {
                V = table.Computer.Strategy.Value + Get0To9();
                if (table.Computer.Balance - table.Human.Bet - V < 0)
                {
                    if (table.Human.Bet == 0)
                    {
                        V = table.Computer.Balance;
                    }
                    else if (table.Computer.Balance - table.Human.Bet >= 0)
                    {
                        _io.WriteLine("I'll see you.");
                        table.Computer.Bet = table.Human.Bet;
                        table.UpdatePot();
                    }
                    else if (!table.Computer.TrySellWatch())
                    {
                        _io.WriteLine("I'm busted.  Congratulations!");
                        return true;
                    }
                }
                _io.WriteLine($"I'll open with ${V}");
                table.Computer.Bet = V;
            }
            else
            {
                _io.WriteLine("I check.");
            }
            if (GetWager(table.Computer.Strategy)) { return true; }
            if (table.SomeoneHasFolded()) { return false; }

            table.Draw();

            table.Computer.Strategy = (table.Computer.Hand.IsWeak, table.Computer.Hand.Rank < HandRank.Three, table.Computer.Hand.Rank < HandRank.FullHouse) switch
            {
                _ when table.Computer.Strategy is Bluff => Strategy.Bluff(28),
                (true, _, _) => Strategy.Fold,
                (false, true, _) => Get0To9() == 0 ? Strategy.Bet(19) : Strategy.Raise,
                (false, false, true) => Get0To9() == 0 ? Strategy.Bet(11) : Strategy.Bet(19),
                (false, false, false) => Strategy.Raise
            };

            if (GetWager(table.Computer.Strategy)) { return true; }
            if (table.Human.HasBet)
            {
                if (table.SomeoneHasFolded()) { return false; }
            }
            else if (table.Computer.Strategy is Strategies.Bet)
            {
                V = table.Computer.Strategy.Value + Get0To9();
                if (table.Computer.Balance - table.Human.Bet - V < 0)
                {
                    if (table.Human.Bet == 0)
                    {
                        V = table.Computer.Balance;
                    }
                    else if (table.Computer.Balance - table.Human.Bet >= 0)
                    {
                        _io.WriteLine("I'll see you.");
                        table.Computer.Bet = table.Human.Bet;
                        table.UpdatePot();
                    }
                    else if (!table.Computer.TrySellWatch())
                    {
                        _io.WriteLine("I'm busted.  Congratulations!");
                        return true;
                    }
                }
                _io.WriteLine($"I'll bet ${V}");
                table.Computer.Bet = V;
                if (GetWager(table.Computer.Strategy)) { return true; }
                if (table.SomeoneHasFolded()) { return false; }
            }
            else
            {
                _io.WriteLine("I'll check");
            }
            if (table.GetWinner() is { } winner)
            {
                winner.TakeWinnings();
                return false;
            }
        }

        bool GetWager(Strategy computerStrategy)
        {
            while (true)
            {
                table.Human.HasBet = false;
                while (true)
                {
                    var humanStrategy = _io.ReadHumanStrategy(table.Computer.Bet == 0 && table.Human.Bet == 0);
                    if (humanStrategy is Bet or Check)
                    {
                        if (table.Human.Bet + humanStrategy.Value < table.Computer.Bet)
                        {
                            _io.WriteLine("If you can't see my bet, then fold.");
                            continue;
                        }
                        if (table.Human.Balance - table.Human.Bet - humanStrategy.Value >= 0)
                        {
                            table.Human.HasBet = true;
                            table.Human.Bet += humanStrategy.Value;
                            break;
                        }
                        if (table.Human.IsBroke()) { return true; }
                        continue;
                    }
                    else
                    {
                        table.Human.Fold();
                        table.UpdatePot();
                        return false;
                    }
                }
                if (table.Human.Bet == table.Computer.Bet)
                {
                    table.UpdatePot();
                    return false;
                }
                if (computerStrategy is Fold)
                {
                    if (table.Human.Bet > 5)
                    {
                        table.Computer.Fold();
                        _io.WriteLine("I fold.");
                        return false;
                    }
                    V = 5;
                }
                if (table.Human.Bet > 3 * computerStrategy.Value)
                {
                    if (computerStrategy is not Raise)
                    {
                        _io.WriteLine("I'll see you.");
                        table.Computer.Bet = table.Human.Bet;
                        table.UpdatePot();
                        return false;
                    }
                }

                V = table.Human.Bet - table.Computer.Bet + Get0To9();
                if (table.Computer.Balance - table.Human.Bet - V < 0)
                {
                    if (table.Human.Bet == 0)
                    {
                        V = table.Computer.Balance;
                    }
                    else if (table.Computer.Balance - table.Human.Bet >= 0)
                    {
                        _io.WriteLine("I'll see you.");
                        table.Computer.Bet = table.Human.Bet;
                        table.UpdatePot();
                    }
                    else if (!table.Computer.TrySellWatch())
                    {
                        _io.WriteLine("I'm busted.  Congratulations!");
                        return true;
                    }
                }
                _io.WriteLine($"I'll see you, and raise you{V}");
                table.Computer.Bet = table.Human.Bet + V;
            }
        }
    }
}
