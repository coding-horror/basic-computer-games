using System.Collections.Immutable;

namespace Roulette;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private readonly Table _table;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
        _table = new Table(io, random);
    }

    public void Play()
    {
        _io.Write(Streams.Title);
        if (!_io.ReadString(Prompts.Instructions).ToLowerInvariant().StartsWith('n'))
        {
            _io.Write(Streams.Instructions);
        }

        while (_table.Play());

        if (_table.Balance > 0)
        {
            var name = _io.ReadString(Prompts.Check);
            _io.Write(Strings.Check(_random, name, _table.Balance));
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
        new Slot(Strings.Red(1)),
        new Slot(Strings.Black(2)),
        new Slot(Strings.Red(3)),
        new Slot(Strings.Black(4)),
        new Slot(Strings.Red(5)),
        new Slot(Strings.Black(6)),
        new Slot(Strings.Red(7)),
        new Slot(Strings.Black(8)),
        new Slot(Strings.Red(9)),
        new Slot(Strings.Black(10)),
        new Slot(Strings.Black(11)),
        new Slot(Strings.Red(12)),
        new Slot(Strings.Black(13)),
        new Slot(Strings.Red(14)),
        new Slot(Strings.Black(15)),
        new Slot(Strings.Red(16)),
        new Slot(Strings.Black(17)),
        new Slot(Strings.Red(18)),
        new Slot(Strings.Red(19)),
        new Slot(Strings.Black(20)),
        new Slot(Strings.Red(21)),
        new Slot(Strings.Black(22)),
        new Slot(Strings.Red(23)),
        new Slot(Strings.Black(24)),
        new Slot(Strings.Red(25)),
        new Slot(Strings.Black(26)),
        new Slot(Strings.Red(27)),
        new Slot(Strings.Black(28)),
        new Slot(Strings.Black(29)),
        new Slot(Strings.Red(30)),
        new Slot(Strings.Black(31)),
        new Slot(Strings.Red(32)),
        new Slot(Strings.Black(33)),
        new Slot(Strings.Red(34)),
        new Slot(Strings.Black(35)),
        new Slot(Strings.Red(36)),
        new Slot("0"),
        new Slot("00"));
    
    private readonly IRandom _random;

    public Wheel(IRandom random) => _random = random;

    public Slot Spin() => _slots[_random.Next(_slots.Length)];
}

internal record struct Slot(string Name);

internal record struct Bet(int Number, int Amount)
{
    public Bet(int number) : this(number, 0) { }

    public bool Equals(Bet? other) => Number == other?.Number;
}

public class Table
{
    private readonly IReadWrite _io;
    private readonly Wheel _wheel;

    private int _houseBalance = 100_000;
    private int _playerBalance = 1_000;

    public Table(IReadWrite io, IRandom random)
    {
        _io = io;
        _wheel = new(random);
    }

    public int Balance => _playerBalance;

    public bool Play()
    {
        var betCount = _io.ReadBetCount();
        var bets = new HashSet<Bet>();
        for (int i = 0; i < betCount; i++)
        {
            while (!bets.Add(_io.ReadBet(i)))
            {
                _io.Write(Streams.BetAlready);
            }
        }
        
        return _io.ReadString(Prompts.Again).ToLowerInvariant().StartsWith('y');
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
            var (bet, amount) = io.Read2Numbers(Prompts.Bet(number));

            if (bet.IsValidInt(1, 50) && amount.IsValidInt(5, 500))
            {
                return new((int)bet, (int)amount);
            }
        }
    }

    internal static bool IsValidInt(this float value, int minValue, int maxValue = int.MaxValue)
        => value == (int)value && value >= minValue && value <= maxValue;
}