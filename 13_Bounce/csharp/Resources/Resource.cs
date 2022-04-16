using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bounce.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Instructions => GetStream();
        public static Stream Title => GetStream();
    }

    private static Stream GetStream([CallerMemberName] string? name = null)
        => Assembly.GetExecutingAssembly().GetManifestResourceStream($"Bounce.Resources.{name}.txt")
            ?? throw new ArgumentException($"Resource stream {name} does not exist", nameof(name));
}