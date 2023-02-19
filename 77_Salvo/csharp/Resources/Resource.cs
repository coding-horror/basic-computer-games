using System.Reflection;
using System.Runtime.CompilerServices;

namespace Salvo.Resources;

internal static class Resource
{
    internal static class Streams
    {
        public static Stream Title => GetStream();
    }

    internal static class Prompts
    {
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