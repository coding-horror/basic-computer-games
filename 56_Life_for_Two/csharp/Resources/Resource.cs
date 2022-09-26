using System.Reflection;
using System.Runtime.CompilerServices;

namespace LifeforTwo.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Title => GetStream();
        public static Stream IllegalCoords => GetStream();
        public static Stream SameCoords => GetStream();
    }

    internal static class Formats
    {
        public static string InitialPieces => GetString();
        public static string Player => GetString();
        public static string Winner => GetString();
    }

    internal static class Strings
    {
        public static string Draw => GetString();
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