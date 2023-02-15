using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Roulette;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private readonly Table _table;
    private readonly House _house;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
        _house = new();
        _table = new(_house, io, random);
    }

    public void Play()
    {
        _io.Write(Streams.Title);
        if (!_io.ReadString(Prompts.Instructions).ToLowerInvariant().StartsWith('n'))
        {
            _io.Write(Streams.Instructions);
        }

        while (_table.Play());

        if (!_house.PlayerIsBroke)
        {
            _house.CutCheck(_io, _random);
        }
        else
        {
            _io.Write(Streams.Thanks);
        }
    }
}

internal class Wheel
{
    private static readonly ImmutableArray<Slot> _slots = ImmutableArray.Create(
        new Slot(Strings.Red(1), 1, 37, 40, 43, 46, 47),
        new Slot(Strings.Black(2), 2, 37, 41, 43, 45, 48),
        new Slot(Strings.Red(3), 3, 37, 42, 43, 46, 47),
        new Slot(Strings.Black(4), 4, 37, 40, 43, 45, 48),
        new Slot(Strings.Red(5), 5, 37, 41, 43, 46, 47),
        new Slot(Strings.Black(6), 6, 37, 42, 43, 45, 48),
        new Slot(Strings.Red(7), 7, 37, 40, 43, 46, 47),
        new Slot(Strings.Black(8), 8, 37, 41, 43, 45, 48),
        new Slot(Strings.Red(9), 9, 37, 42, 43, 46, 47),
        new Slot(Strings.Black(10), 10, 37, 40, 43, 45, 48),
        new Slot(Strings.Black(11), 11, 37, 41, 43, 46, 48),
        new Slot(Strings.Red(12), 12, 37, 42, 43, 45, 47),
        new Slot(Strings.Black(13), 13, 38, 40, 43, 46, 48),
        new Slot(Strings.Red(14), 14, 38, 41, 43, 45, 47),
        new Slot(Strings.Black(15), 15, 38, 42, 43, 46, 48),
        new Slot(Strings.Red(16), 16, 38, 40, 43, 45, 47),
        new Slot(Strings.Black(17), 17, 38, 41, 43, 46, 48),
        new Slot(Strings.Red(18), 18, 38, 42, 43, 45, 47),
        new Slot(Strings.Red(19), 19, 38, 40, 44, 46, 47),
        new Slot(Strings.Black(20), 20, 38, 41, 44, 45, 48),
        new Slot(Strings.Red(21), 21, 38, 42, 44, 46, 47),
        new Slot(Strings.Black(22), 22, 38, 40, 44, 45, 48),
        new Slot(Strings.Red(23), 23, 38, 41, 44, 46, 47),
        new Slot(Strings.Black(24), 24, 38, 42, 44, 45, 48),
        new Slot(Strings.Red(25), 25, 39, 40, 44, 46, 47),
        new Slot(Strings.Black(26), 26, 39, 41, 44, 45, 48),
        new Slot(Strings.Red(27), 27, 39, 42, 44, 46, 47),
        new Slot(Strings.Black(28), 28, 39, 40, 44, 45, 48),
        new Slot(Strings.Black(29), 29, 39, 41, 44, 46, 48),
        new Slot(Strings.Red(30), 30, 39, 42, 44, 45, 47),
        new Slot(Strings.Black(31), 31, 39, 40, 44, 46, 48),
        new Slot(Strings.Red(32), 32, 39, 41, 44, 45, 47),
        new Slot(Strings.Black(33), 33, 39, 42, 44, 46, 48),
        new Slot(Strings.Red(34), 34, 39, 40, 44, 45, 47),
        new Slot(Strings.Black(35), 35, 39, 41, 44, 46, 48),
        new Slot(Strings.Red(36), 36, 39, 42, 44, 45, 47),
        new Slot("0", 49),
        new Slot("00", 50));
    
    private readonly IRandom _random;

    public Wheel(IRandom random) => _random = random;

    public Slot Spin() => _slots[_random.Next(_slots.Length)];
}

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

internal record struct BetType(int Value)
{
    public static implicit operator BetType(int value) => new(value);

    public int Payout => Value switch
        {
            <= 36 or >= 49 => 35,
            <= 42 => 2,
            <= 48 => 1
        };
}

internal record struct Bet(BetType Type, int Number, int Wager)
{
    public int Payout => Wager * Type.Payout;
}

public class Table
{
    private readonly IReadWrite _io;
    private readonly Wheel _wheel;
    private readonly House _house;

    public Table(House house, IReadWrite io, IRandom random)
    {
        _house = house;
        _io = io;
        _wheel = new(random);
    }

    public bool Play()
    {
        var bets = AcceptBets();
        var slot = SpinWheel();
        SettleBets(bets, slot);

        _io.Write(_house.Totals);

        if (_house.PlayerIsBroke)
        {
            _io.Write(Streams.LastDollar);
            _io.Write(Streams.Thanks);
            return false;
        }

        if (_house.HouseIsBroke)
        {
            _io.Write(Streams.BrokeHouse);
            return false;
        }

        return _io.ReadString(Prompts.Again).ToLowerInvariant().StartsWith('y');
    }

    private Slot SpinWheel()
    {
        _io.Write(Streams.Spinning);
        var slot = _wheel.Spin();
        _io.Write(slot.Name);
        return slot;
    }

    private IReadOnlyList<Bet> AcceptBets()
    {
        var betCount = _io.ReadBetCount();
        var betTypes = new HashSet<BetType>();
        var bets = new List<Bet>();
        for (int i = 1; i <= betCount; i++)
        {
            while (!TryAdd(_io.ReadBet(i)))
            {
                _io.Write(Streams.BetAlready);
            }
        }

        return bets.AsReadOnly();

        bool TryAdd(Bet bet)
        {
            if (betTypes.Add(bet.Type))
            {
                bets.Add(bet);
                return true;
            }

            return false;
        }
    }

    private void SettleBets(IReadOnlyList<Bet> bets, Slot slot)
    {
        foreach (var bet in bets)
        {
            _io.Write(slot.IsCoveredBy(bet) ? _house.Pay(bet) : _house.Take(bet));
        }
    }
}

public class House
{
    private const int _initialHouse = 100_000;
    private const int _initialPlayer = 1_000;

    private int _balance = _initialHouse;
    private int _player = _initialPlayer;

    public string Totals => Strings.Totals(_balance, _player);
    public bool PlayerIsBroke => _player <= 0;
    public bool HouseIsBroke => _balance <= 0;

    internal string Pay(Bet bet)
    {
        _balance -= bet.Payout;
        _player += bet.Payout;

        if (_balance <= 0)
        {
            _player = _initialHouse + _initialPlayer;
        }

        return Strings.Win(bet);
    }

    internal string Take(Bet bet)
    {
        _balance += bet.Wager;
        _player -= bet.Wager;

        return Strings.Lose(bet);
    }

    public void CutCheck(IReadWrite io, IRandom random)
    {
        var name = io.ReadString(Prompts.Check);
        io.Write(Strings.Check(random, name, _player));
    }
}

internal static class IOExtensions
{
    internal static int ReadBetCount(this IReadWrite io)
    {
        while (true)
        {
            var betCount = io.ReadNumber(Prompts.HowManyBets);
            if (betCount.IsValidInt(1)) { return (int)betCount; }
        }
    }

    internal static Bet ReadBet(this IReadWrite io, int number)
    {
        while (true)
        {
            var (type, amount) = io.Read2Numbers(Prompts.Bet(number));

            if (type.IsValidInt(1, 50) && amount.IsValidInt(5, 500))
            {
                return new()
                {
                    Type = (int)type, 
                    Number = number, 
                    Wager = (int)amount
                };
            }
        }
    }

    internal static bool IsValidInt(this float value, int minValue, int maxValue = int.MaxValue)
        => value == (int)value && value >= minValue && value <= maxValue;
}