using static System.IO.Directory;
using static System.IO.Path;
using static DotnetUtils.Globals;

namespace DotnetUtils;

public record PortInfo(
    string GamePath, string FolderName, int Index, string GameName,
    string LangPath, string Lang, string Ext, string ProjExt,
    string[] CodeFiles, string[] Slns, string[] Projs
) {
    
    private static readonly EnumerationOptions enumerationOptions = new() {
        RecurseSubdirectories = true,
        MatchType = MatchType.Simple,
        MatchCasing = MatchCasing.CaseInsensitive
    };

    // .NET namespaces cannot have a digit as the first character
    // For games whose name starts with a digit, we map the name to a specific string
    private static readonly Dictionary<string, string> specialGameNames = new() {
        { "3-D_Plot", "Plot" },
        { "3-D_Tic-Tac-Toe", "ThreeDTicTacToe" },
        { "23_Matches", "TwentyThreeMatches"}
    };

    public static PortInfo? Create(string gamePath, string langKeyword) {
        var folderName = GetFileName(gamePath);
        var parts = folderName.Split('_', 2);

        if (parts.Length <= 1) { return null; }

        var (index, gameName) = (
            int.TryParse(parts[0], out var n) && n > 0 ? // ignore utilities folder
                n :
                (int?)null,
            specialGameNames.TryGetValue(parts[1], out var specialName) ? 
                specialName :
                parts[1].Replace("_", "").Replace("-", "")
        );

        if (index is null || gameName is null) { return null; }

        var (ext, projExt) = LangData[langKeyword];
        var langPath = Combine(gamePath, langKeyword);
        var codeFiles =
            GetFiles(langPath, $"*.{ext}", enumerationOptions)
                .Where(x => !x.Contains("\\bin\\") && !x.Contains("\\obj\\"))
                .ToArray();

        return new PortInfo(
            gamePath, folderName, index.Value, gameName,
            langPath, langKeyword, ext, projExt,
            codeFiles,
            GetFiles(langPath, "*.sln", enumerationOptions),
            GetFiles(langPath, $"*.{projExt}", enumerationOptions)
        );
    }
}
