using System.Reflection;
using System.Runtime.CompilerServices;

namespace OneCheck.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Introduction => GetStream();
        public static Stream IllegalMove => GetStream();
        public static Stream YesOrNo => GetStream();
        public static Stream Bye => GetStream();
    }

    internal static class Formats
    {
        public static string Results => GetString();
    }

    internal static class Prompts
    {
        public static string From => GetString();
        public static string To => GetString();
        public static string TryAgain => GetString();
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