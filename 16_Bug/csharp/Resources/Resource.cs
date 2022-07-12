using System.Reflection;
using System.Runtime.CompilerServices;

namespace BugGame.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Introduction => GetStream();
        public static Stream Instructions => GetStream();
        public static Stream PlayAgain => GetStream();
    }

    private static Stream GetStream([CallerMemberName] string? name = null) =>
        Assembly.GetExecutingAssembly()
            .GetManifestResourceStream($"Bug.Resources.{name}.txt")
            ?? throw new Exception($"Could not find embedded resource stream '{name}'.");
}