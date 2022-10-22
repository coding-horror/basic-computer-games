using System.Reflection;
using System.Runtime.CompilerServices;

namespace King.Resources;

internal static class Resource
{
    public static Stream Title => GetStream();
    
    public static string InstructionsPrompt => GetString();
    public static string InstructionsText(int years) => string.Format(GetString(), years);

    public static string SavedYearsPrompt => GetString();
    public static string SavedYearsError(int years) => string.Format(GetString(), years);
    public static string SavedTreasuryPrompt => GetString();
    public static string SavedCountrymenPrompt => GetString();
    public static string SavedWorkersPrompt => GetString();
    public static string SavedLandPrompt => GetString();
    public static string SavedLandError => GetString();
    
    internal static class Formats
    {
        public static string Player => GetString();
        public static string YouLose => GetString();
    }

    internal static class Prompts
    {
        public static string WantInstructions => GetString();
        public static string HowManyPlayers => GetString();
        public static string HowManyRows => GetString();
        public static string HowManyColumns => GetString();
        public static string TooManyColumns => GetString();
    }

    internal static class Strings
    {
        public static string TooManyColumns => GetString();
        public static string TooManyRows => GetString();
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