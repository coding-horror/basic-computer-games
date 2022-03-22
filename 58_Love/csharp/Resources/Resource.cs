using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Love.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Intro => GetStream();
    }

    private static Stream GetStream([CallerMemberName] string name = null)
        => Assembly.GetExecutingAssembly().GetManifestResourceStream($"Love.Resources.{name}.txt");
}