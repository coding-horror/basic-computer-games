using System.Reflection;
using System.Runtime.CompilerServices;

namespace Digits.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Introduction => GetStream();
        public static Stream Instructions => GetStream();
        public static Stream TryAgain => GetStream();
        public static Stream ItsATie => GetStream();
        public static Stream IWin => GetStream();
        public static Stream YouWin => GetStream();
        public static Stream Thanks => GetStream();
        public static Stream Headings => GetStream();
    }

    internal static class Prompts
    {
        public static string ForInstructions => GetString();
        public static string TenNumbers => GetString();
        public static string WantToTryAgain => GetString();
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