using Poker.Cards;
using Poker.Players;
using Poker.Resources;

namespace Poker;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    private int _playerBet;
    private int _playerTotalBet;
    private int Z;
    private int _computerTotalBet;
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

        do
        {
            deck.Shuffle(_random);
        } while (PlayHand(table));
    }

    internal bool PlayHand(Table table)
    {
        while(true)
        {
            _io.WriteLine();
            if (table.Computer.Balance<=5)
            {
                CongratulatePlayer();
                return false;
            }
            _io.WriteLine("The ante is $5.  I will deal:");
            _io.WriteLine();
            if (table.Human.Balance <= 5 && table.Human.IsBroke()) { return false; }

            table.Deal();

            _io.WriteLine();
            Z = table.Computer.Hand.Rank switch
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
                _computerTotalBet = 0;
                _io.WriteLine("I check.");
            }
            else
            {
                V=Z+Get0To9();
                if (ComputerCantContinue()) { return false; }
                _io.WriteLine($"I'll open with ${V}");
                _computerTotalBet = V;
                _playerTotalBet = 0;
            }
            if (GetWager()) { return false; }
            if (table.SomeoneHasFolded()) { return ShouldContinue(); }

            table.Draw();

            Z = table.Computer.Hand.Rank switch
            {
                _ when table.Computer.IsBluffing => 28,
                _ when table.Computer.Hand.IsWeak => 1,
                _ when table.Computer.Hand.Rank < HandRank.Three => Get0To9() == 0 ? 19 : 2,
                _ when table.Computer.Hand.Rank < HandRank.FullHouse => Get0To9() == 0 ? 11 : 19,
                _ => 2
            };

            _computerTotalBet = 0;
            _playerTotalBet = 0;
            if (GetWager()) { return false; }
            if (_playerBet != 0)
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
                if (ComputerCantContinue()) { return false; }
                _io.WriteLine($"I'll bet ${V}");
                _computerTotalBet = V;
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
            _playerBet = 0;
            while(true)
            {
                if (_io.ReadPlayerAction(_computerTotalBet == 0 && _playerTotalBet == 0) is Bet bet)
                {
                    if (_playerTotalBet + bet.Amount < _computerTotalBet)
                    {
                        _io.WriteLine("If you can't see my bet, then fold.");
                        continue;
                    }
                    if (table.Human.Balance - _playerTotalBet - bet.Amount >= 0)
                    {
                        _playerBet = bet.Amount;
                        _playerTotalBet += bet.Amount;
                        break;
                    }
                    if (table.Human.IsBroke()) { return true; }
                    continue;
                }
                else
                {
                    table.Human.Fold();
                    return UpdatePot();
                }
            }
            if (_playerTotalBet == _computerTotalBet) { return UpdatePot(); }
            if (Z == 1)
            {
                if (_playerTotalBet > 5)
                {
                    table.Computer.Fold();
                    _io.WriteLine("I fold.");
                    return false;
                }
                V = 5;
            }
            return Line_3420();
        }

        bool Line_3350()
        {
            if (Z==2) { return Line_3430(); }
            return Line_3360();
        }

        bool Line_3360()
        {
            _io.WriteLine("I'll see you.");
            _computerTotalBet = _playerTotalBet;
            return UpdatePot();
        }

        bool UpdatePot()
        {
            table.Human.Balance -= _playerTotalBet;
            table.Computer.Balance -= _computerTotalBet;
            table.Pot += _playerTotalBet + _computerTotalBet;
            return false;
        }

        bool Line_3420()
        {
            if (_playerTotalBet>3*Z) { return Line_3350(); }
            return Line_3430();
        }

        bool Line_3430()
        {
            V = _playerTotalBet - _computerTotalBet + Get0To9();
            if (ComputerCantContinue()) { return true; }
            _io.WriteLine($"I'll see you, and raise you{V}");
            _computerTotalBet = _playerTotalBet + V;
            return GetWager();
        }

        bool ComputerCantContinue()
        {
            if (table.Computer.Balance - _playerTotalBet - V >= 0) { return false; }
            if (_playerTotalBet == 0)
            {
                V = table.Computer.Balance;
            }
            else if (table.Computer.Balance - _playerTotalBet >= 0)
            {
                return Line_3360();
            }
            else if (table.Computer.TrySellWatch(table.Human))
            {
                return false;
            }
            return CongratulatePlayer();
        }

        bool CongratulatePlayer()
        {
            _io.WriteLine("I'm busted.  Congratulations!");
            return true;
        }
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
