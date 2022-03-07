namespace Boxing;
public static class GameUtils
{
    private static readonly Random Rnd = new((int) DateTime.UtcNow.Ticks);
    public static void PrintPunchDescription() =>
        Console.WriteLine($"DIFFERENT PUNCHES ARE: {PunchDesc(Punch.FullSwing)}; {PunchDesc(Punch.Hook)}; {PunchDesc(Punch.Uppercut)}; {PunchDesc(Punch.Jab)}.");

    private static string PunchDesc(Punch punch) => $"({(int)punch}) {punch.ToFriendlyString()}";

    public static Punch GetPunch(string prompt)
    {
        Console.WriteLine(prompt);
        Punch result;
        while (!Enum.TryParse(Console.ReadLine(), out result) || !Enum.IsDefined(typeof(Punch), result))
        {
            PrintPunchDescription();
        }
        return result;
    }

    public static Func<int, int> Roll { get;  } =  upperLimit => (int) (upperLimit * Rnd.NextSingle()) + 1;

    public static bool RollSatisfies(int upperLimit, Predicate<int> predicate) => predicate(Roll(upperLimit));

    public static string ToFriendlyString(this Punch punch)
        => punch switch
        {
            Punch.FullSwing => "FULL SWING",
            Punch.Hook => "HOOK",
            Punch.Uppercut => "UPPERCUT",
            Punch.Jab => "JAB",
            _ => throw new ArgumentOutOfRangeException(nameof(punch), punch, null)
        };

}
