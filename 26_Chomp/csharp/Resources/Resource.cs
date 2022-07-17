using System.Reflection;
using System.Runtime.CompilerServices;

namespace Chomp.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream HereWeGo => GetStream();
        public static Stream Introduction => GetStream();
        public static Stream Rules => GetStream();
        public static Stream NoFair => GetStream();
    }

    internal static class Formats
    {
        public static string Player => GetString();
        public static string YouLose => GetString();
    }

    internal static class Prompts
    {
        public static string Coordinates => GetString();
        public static string HowManyPlayers => GetString();
        public static string HowManyRows => GetString();
        public static string HowManyColumns => GetString();
        public static string TooManyColumns => GetString();
    }

    internal static class Strings
    {
        public static string TooManyColumns => GetString();
        public static string TooManyRows => GetString();
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