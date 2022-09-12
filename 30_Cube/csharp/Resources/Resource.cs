using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cube.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Introduction => GetStream();
        public static Stream Instructions => GetStream();
        public static Stream Wager => GetStream();
        public static Stream IllegalMove => GetStream();
        public static Stream Bang => GetStream();
        public static Stream Bust => GetStream();
        public static Stream Congratulations => GetStream();
        public static Stream Goodbye => GetStream();
    }

    internal static class Prompts
    {
        public static string HowMuch => GetString();
        public static string BetAgain => GetString();
        public static string YourMove => GetString();
        public static string NextMove => GetString();
        public static string TryAgain => GetString();
    }

    internal static class Formats
    {
        public static string Balance => GetString();
    }

    private static string GetString([CallerMemberName] string? name = null)
    {
        using var stream = GetStream(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static Stream GetStream([CallerMemberName] string? name = null) =>
        Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(Resource).Namespace}.{name}.txt")
            ?? throw new Exception($"Could not find embedded resource stream '{name}'.");
}