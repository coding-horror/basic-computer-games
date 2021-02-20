using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

using static System.StringComparison;

namespace SuperStarTrek
{
    internal static class Strings
    {
        public static string Title => GetResource();
        public static string Instructions => GetResource();
        public static string Enterprise => GetResource();

        private static string GetResource([CallerMemberName] string name = "")
        {
            var streamName = $"SuperStarTrek.Resources.{name}.txt";
            using var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream(streamName);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
