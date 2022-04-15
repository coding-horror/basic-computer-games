using System.Reflection;
using System.Runtime.CompilerServices;

namespace Chief.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Bye => GetStream();
        public static Stream Instructions => GetStream();
        public static Stream Lightning => GetStream();
        public static Stream ShutUp => GetStream();
        public static Stream Title => GetStream();
    }

    internal static class Formats
    {
        public static string Bet => GetString();
        public static string Working => GetString();
    }

    internal static class Prompts
    {
        public static string Answer => GetString();
        public static string Believe => GetString();
        public static string Original => GetString();
        public static string Ready => GetString();
    }

    private static string GetString([CallerMemberName] string? name = null)
    {
        using var stream = GetStream(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static Stream GetStream([CallerMemberName] string? name = null)
        => Assembly.GetExecutingAssembly().GetManifestResourceStream($"Chief.Resources.{name}.txt")
            ?? throw new ArgumentException($"Resource stream {name} does not exist", nameof(name));
}