using static Chief.Resources.Resource;

namespace Chief;

public static class Math
{
    public static float CalculateOriginal(float result) => (result + 1 - 5) * 5 / 8 * 5 - 3;

    public static string ShowWorking(float value) =>
        string.Format(
            Formats.Working,
            value,
            value += 3,
            value /= 5,
            value *= 8,
            value = value / 5 + 5,
            value - 1);
}