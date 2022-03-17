using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Hexapawn.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Instructions => GetStream();
        public static Stream Title => GetStream();
    }

    private static Stream GetStream([CallerMemberName] string name = null)
        => Assembly.GetExecutingAssembly().GetManifestResourceStream($"Hexapawn.Resources.{name}.txt");
}