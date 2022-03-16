using System;
using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions
{
    internal class CumulativeGalacticRecord : GalacticReport
    {
        internal CumulativeGalacticRecord(Output output, Galaxy galaxy)
            : base("Cumulative galactic record", output, galaxy)
        {
        }

        protected override void WriteHeader(Quadrant quadrant) =>
            Output.NextLine().WriteLine($"Computer record of galaxy for quadrant {quadrant.Coordinates}").NextLine();

        protected override IEnumerable<string> GetRowData() =>
            Galaxy.Quadrants.Select(row => " " + string.Join("   ", row));
    }
}
