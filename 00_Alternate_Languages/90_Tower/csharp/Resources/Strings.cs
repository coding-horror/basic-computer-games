using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Tower.Resources
{
    internal static class Strings
    {
        internal static string Congratulations => GetResource();
        internal static string DiskCountPrompt => GetResource();
        internal static string DiskCountQuit => GetResource();
        internal static string DiskCountRetry => GetResource();
        internal static string DiskNotInPlay => GetResource();
        internal static string DiskPrompt => GetResource();
        internal static string DiskQuit => GetResource();
        internal static string DiskRetry => GetResource();
        internal static string DiskUnavailable => GetResource();
        internal static string IllegalMove => GetResource();
        internal static string Instructions => GetResource();
        internal static string Intro => GetResource();
        internal static string NeedlePrompt => GetResource();
        internal static string NeedleQuit => GetResource();
        internal static string NeedleRetry => GetResource();
        internal static string PlayAgainPrompt => GetResource();
        internal static string TaskFinished => GetResource();
        internal static string Thanks => GetResource();
        internal static string Title => GetResource();
        internal static string TooManyMoves => GetResource();
        internal static string YesNoPrompt => GetResource();

        private static string GetResource([CallerMemberName] string name = "")
        {
            var streamName = $"Tower.Resources.{name}.txt";
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(streamName);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
