using System.Collections.Generic;
using System.Linq;
using Games.Common.IO;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions;

internal class CumulativeGalacticRecord : GalacticReport
{
    internal CumulativeGalacticRecord(IReadWrite io, Galaxy galaxy)
        : base("Cumulative galactic record", io, galaxy)
    {
    }

    protected override void WriteHeader(Quadrant quadrant)
    {
        IO.WriteLine();
        IO.WriteLine($"Computer record of galaxy for quadrant {quadrant.Coordinates}");
        IO.WriteLine();
    }

    protected override IEnumerable<string> GetRowData() =>
        Galaxy.Quadrants.Select(row => " " + string.Join("   ", row));
}
