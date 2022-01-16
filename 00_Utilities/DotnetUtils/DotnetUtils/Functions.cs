using System.Xml.Linq;

namespace DotnetUtils;

public static class Functions {
    public static string? getValue(string path, params string[] names) {
        if (names.Length == 0) { throw new InvalidOperationException(); }
        var parent = XDocument.Load(path).Element("Project")?.Element("PropertyGroup");
        return getValue(parent, names);
    }

    public static string? getValue(XElement? parent, params string[] names) {
        if (names.Length == 0) { throw new InvalidOperationException(); }
        XElement? elem = null;
        foreach (var name in names) {
            elem = parent?.Element(name);
            if (elem != null) { break; }
        }
        return elem?.Value;
    }
}
