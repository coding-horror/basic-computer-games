namespace DotnetUtils;

public static class Globals {
    public static readonly Dictionary<string, (string codefileExtension, string projExtension)> LangData = new() {
        { "csharp", ("cs", "csproj") },
        { "vbnet", ("vb", "vbproj") }
    };
}
