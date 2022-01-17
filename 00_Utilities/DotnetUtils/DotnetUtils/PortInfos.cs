using System.Reflection;
using static System.IO.Directory;
using static DotnetUtils.Globals;

namespace DotnetUtils;

public static class PortInfos {
    public static readonly string Root;

    static PortInfos() {
        Root = GetParent(Assembly.GetEntryAssembly()!.Location)!.FullName;
        Root = Root[..Root.IndexOf(@"\00_Utilities")];

        Get = GetDirectories(Root)
            .SelectMany(gamePath => LangData.Keys.Select(keyword => (gamePath, keyword)))
            .SelectT((gamePath, keyword) => PortInfo.Create(gamePath, keyword))
            .Where(x => x is not null)
            .ToArray()!;
    }

    public static readonly PortInfo[] Get;
}
