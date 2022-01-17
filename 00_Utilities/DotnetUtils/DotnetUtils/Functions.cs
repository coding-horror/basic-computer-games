using System.Xml.Linq;
using static System.Console;

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

    public static int getChoice(int maxValue) => getChoice(0, maxValue);

    public static int getChoice(int minValue, int maxValue) {
        int result;
        do {
            Write("? ");
        } while (!int.TryParse(ReadLine(), out result) || result < minValue || result > maxValue);
        //WriteLine();
        return result;
    }


}
