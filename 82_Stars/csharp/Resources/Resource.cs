using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Stars.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Title => GetStream();
    }

    internal static class Formats
    {
        public static string Instructions => GetString();
    }

    private static string GetString([CallerMemberName] string name = null)
    {
        using var stream = GetStream(name);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static Stream GetStream([CallerMemberName] string name = null)
        => Assembly.GetExecutingAssembly().GetManifestResourceStream($"Stars.Resources.{name}.txt");
}