using System.Reflection;
using System.Runtime.CompilerServices;

namespace Queen.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Title => GetStream();
        public static Stream Instructions => GetStream();
        public static Stream YesOrNo => GetStream();
        public static Stream IllegalStart => GetStream();
        public static Stream ComputerMove => GetStream();
        public static Stream IllegalMove => GetStream();
        public static Stream Forfeit => GetStream();
        public static Stream IWin => GetStream();
        public static Stream Congratulations => GetStream();
        public static Stream Thanks => GetStream();
    }

    internal static class Prompts
    {
        public static string Instructions => GetPrompt();
        public static string Start => GetPrompt();
        public static string Move => GetPrompt();
        public static string Anyone => GetPrompt();
    }

    internal static class Formats
    {
        public static string Balance => GetString();
    }

    private static string GetPrompt([CallerMemberName] string? name = null) => GetString($"{name}Prompt");

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