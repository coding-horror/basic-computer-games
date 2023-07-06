using System.Collections.Immutable;

namespace Roulette;

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
