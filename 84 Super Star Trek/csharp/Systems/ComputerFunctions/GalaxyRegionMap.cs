using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions
{
    internal class GalaxyRegionMap : GalacticReport
    {
        internal GalaxyRegionMap(Output output, Galaxy galaxy)
            : base("Galaxy 'region name' map", output, galaxy)
        {
        }

        protected override void WriteHeader(Quadrant quadrant) =>
            Output.WriteLine("                        The Galaxy");

        protected override IEnumerable<string> GetRowData() =>
            Strings.RegionNames.Split('\n').Select(n => n.TrimEnd('\r'));
    }
}