using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SuperStarTrek.Resources
{
    internal static class Strings
    {
        public static string CombatArea => GetResource();
        public static string ComputerFunctions => GetResource();
        public static string Congratulations => GetResource();
        public static string CourtMartial => GetResource();
        public static string Destroyed => GetResource();
        public static string EndOfMission => GetResource();
        public static string Enterprise => GetResource();
        public static string Instructions => GetResource();
        public static string LowShields => GetResource();
        public static string NoEnemyShips => GetResource();
        public static string NoStarbase => GetResource();
        public static string Orders => GetResource();
        public static string Protected => GetResource();
        public static string RegionNames => GetResource();
        public static string RelievedOfCommand => GetResource();
        public static string RepairEstimate => GetResource();
        public static string RepairPrompt => GetResource();
        public static string ReplayPrompt => GetResource();
        public static string ShieldsDropped => GetResource();
        public static string ShortRangeSensorsOut => GetResource();
        public static string StartText => GetResource();
        public static string Stranded => GetResource();
        public static string Title => GetResource();

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
