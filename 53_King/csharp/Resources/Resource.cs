using System.Reflection;
using System.Runtime.CompilerServices;

namespace King.Resources;

internal static class Resource
{
    private static bool _sellLandErrorShown;

    public static Stream Title => GetStream();
    
    public static string InstructionsPrompt => GetString();
    public static string InstructionsText(int years) => string.Format(GetString(), years);

    public static string Status(
        float rallods,
        float countrymen,
        float workers,
        float land,
        float landValue,
        float plantingCost)
        => string.Format(
            workers == 0 ? StatusWithWorkers : StatusSansWorkers,
            rallods,
            (int)countrymen,
            (int)workers,
            (int)land,
            landValue,
            plantingCost);

    private static string StatusWithWorkers => GetString();
    private static string StatusSansWorkers => GetString();

    public static string SellLandPrompt => GetString();
    public static string SellLandError(float farmLand)
    {
        var error = string.Format(GetString(), farmLand, _sellLandErrorShown ? "" : SellLandErrorReason);
        _sellLandErrorShown = true;
        return error;
    }
    private static string SellLandErrorReason => GetString();

    public static string GiveRallodsPrompt => GetString();
    public static string GiveRallodsError(float rallods) => string.Format(GetString(), rallods);

    public static string PlantLandPrompt => GetString();
    public static string PlantLandError1 => GetString();
    public static string PlantLandError2(float farmLand) => string.Format(GetString(), farmLand);
    public static string PlantLandError3(float rallods) => string.Format(GetString(), rallods);

    public static string PollutionPrompt => GetString();
    public static string PollutionError(float rallods) => string.Format(GetString(), rallods);

    public static string SavedYearsPrompt => GetString();
    public static string SavedYearsError(int years) => string.Format(GetString(), years);
    public static string SavedTreasuryPrompt => GetString();
    public static string SavedCountrymenPrompt => GetString();
    public static string SavedWorkersPrompt => GetString();
    public static string SavedLandPrompt => GetString();
    public static string SavedLandError => GetString();

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