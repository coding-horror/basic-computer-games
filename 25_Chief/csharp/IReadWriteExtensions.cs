namespace Chief;

internal static class IReadWriteExtensions
{
    internal static bool ReadYes(this IReadWrite io, string format, float value) =>
        io.ReadYes(string.Format(format, value));
    internal static bool ReadYes(this IReadWrite io, string prompt) =>
        io.ReadString(prompt).Equals("Yes", StringComparison.InvariantCultureIgnoreCase);
}