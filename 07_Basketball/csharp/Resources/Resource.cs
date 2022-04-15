using System.Reflection;
using System.Runtime.CompilerServices;

namespace Basketball.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Introduction => GetStream();
        public static Stream TwoMinutesLeft => GetStream();
    }

    internal static class Formats
    {
        public static string EndOfFirstHalf => GetString();
        public static string EndOfGame => GetString();
        public static string EndOfSecondHalf => GetString();
        public static string Score => GetString();
    }

    private static string GetString([CallerMemberName] string? name = null)
    {
        using var stream = GetStream(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static Stream GetStream([CallerMemberName] string? name = null) =>
        Assembly.GetExecutingAssembly().GetManifestResourceStream($"Basketball.Resources.{name}.txt")
            ?? throw new Exception($"Could not find embedded resource stream '{name}'.");
}