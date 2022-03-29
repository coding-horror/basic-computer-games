using System.Collections.Generic;
using System.Linq;
using Games.Common.IO;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions;

internal class GalaxyRegionMap : GalacticReport
{
    internal GalaxyRegionMap(IReadWrite io, Galaxy galaxy)
        : base("Galaxy 'region name' map", io, galaxy)
    {
    }

    protected override void WriteHeader(Quadrant quadrant) =>
        IO.WriteLine("                        The Galaxy");

    protected override IEnumerable<string> GetRowData() =>
        Strings.RegionNames.Split('\n').Select(n => n.TrimEnd('\r'));
}
