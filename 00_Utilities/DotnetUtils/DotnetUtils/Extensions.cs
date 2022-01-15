using System.Diagnostics.CodeAnalysis;

namespace DotnetUtils;

public static class Extensions {
    public static IEnumerable<TResult> SelectT<T1, T2, TResult>(this IEnumerable<(T1, T2)> src, Func<T1, T2, TResult> selector) =>
        src.Select(x => selector(x.Item1, x.Item2));
    public static IEnumerable<TResult> SelectT<T1, T2, T3, TResult>(this IEnumerable<(T1, T2, T3)> src, Func<T1, T2, T3, TResult> selector) =>
        src.Select(x => selector(x.Item1, x.Item2, x.Item3));
    public static IEnumerable<(T1, T2, int)> WithIndex<T1, T2>(this IEnumerable<(T1, T2)> src) => src.Select((x, index) => (x.Item1, x.Item2, index));

    public static bool IsNullOrWhitespace([NotNullWhen(false)] this string? s) => string.IsNullOrWhiteSpace(s);

    [return: NotNullIfNotNull("path")]
    public static string? RelativePath(this string? path, string? rootPath) {
        if (
            path.IsNullOrWhitespace() ||
            rootPath.IsNullOrWhitespace()
        ) { return path; }

        var path1 = path.TrimEnd('\\');
        rootPath = rootPath.TrimEnd('\\');
        if (!path1.StartsWith(rootPath, StringComparison.InvariantCultureIgnoreCase)) { return path; }

        return path1[(rootPath.Length + 1)..]; // ignore the initial /
    }
}
