using System.Reflection;
using static System.IO.Directory;
using static System.IO.Path;
using static DotnetUtils.Globals;

namespace DotnetUtils;

public record PortInfo(
    string FullPath, string FolderName, int Index, string GameName,
    string LangPath, string Lang, string Ext, string ProjExt,
    string[] CodeFiles, string[] Slns, string[] Projs
) {
    
    private static readonly EnumerationOptions enumerationOptions = new() {
        RecurseSubdirectories = true,
        MatchType = MatchType.Simple,
        MatchCasing = MatchCasing.CaseInsensitive
    };

    public static PortInfo? Create(string fullPath, string langKeyword) {
        var folderName = GetFileName(fullPath);
        var parts = folderName.Split('_', 2);

        var index =
            parts.Length > 0 && int.TryParse(parts[0], out var n) ?
                n :
                (int?)null;

        var gameName =
            parts.Length > 1 ?
                parts[1].Replace("_", "") :
                null;

        if (index is 0 or null || gameName is null) { return null; }

        var (ext, projExt) = LangData[langKeyword];
        var langPath = Combine(fullPath, langKeyword);
        var codeFiles =
            GetFiles(langPath, $"*.{ext}", enumerationOptions)
                .Where(x => !x.Contains("\\bin\\") && !x.Contains("\\obj\\"))
                .ToArray();

        return new PortInfo(
            fullPath, folderName, index.Value, gameName,
            langPath, langKeyword, ext, projExt,
            codeFiles,
            GetFiles(langPath, "*.sln", enumerationOptions),
            GetFiles(langPath, $"*.{projExt}", enumerationOptions)
        );
    }
}
