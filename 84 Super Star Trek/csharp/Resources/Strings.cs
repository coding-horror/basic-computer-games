using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SuperStarTrek.Resources
{
    internal static class Strings
    {
        internal static string CombatArea => GetResource();

        internal static string ComputerFunctions => GetResource();

        internal static string Congratulations => GetResource();

        internal static string CourtMartial => GetResource();

        internal static string Destroyed => GetResource();

        internal static string EndOfMission => GetResource();

        internal static string Enterprise => GetResource();

        internal static string Instructions => GetResource();

        internal static string LowShields => GetResource();

        internal static string NoEnemyShips => GetResource();

        internal static string NoStarbase => GetResource();

        internal static string NowEntering => GetResource();

        internal static string Orders => GetResource();

        internal static string PermissionDenied => GetResource();

        internal static string Protected => GetResource();

        internal static string RegionNames => GetResource();

        internal static string RelievedOfCommand => GetResource();

        internal static string RepairEstimate => GetResource();

        internal static string RepairPrompt => GetResource();

        internal static string ReplayPrompt => GetResource();

        internal static string ShieldsDropped => GetResource();

        internal static string ShortRangeSensorsOut => GetResource();

        internal static string StartText => GetResource();

        internal static string Stranded => GetResource();

        internal static string Title => GetResource();

        private static string GetResource([CallerMemberName] string name = "")
        {
            var streamName = $"SuperStarTrek.Resources.{name}.txt";
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(streamName);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
